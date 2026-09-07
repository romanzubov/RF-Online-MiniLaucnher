using MiniLauncher.Data;
using MiniLauncherStyle.Data;
using MiniLauncherStyle.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace MiniLauncherStyle.Services
{
    internal class GatewayService : IGatewayService
    {
        private readonly Dictionary<string, GatewayItem> _gatewayCache = new Dictionary<string, GatewayItem>();

        public string GetAddress(string gatewayKey)
        {
            _gatewayCache.TryGetValue(gatewayKey, out var item);

            return string.IsNullOrEmpty(item.Address) ? null : item.Address;
        }

        public void LoadGateways(List<GatewaySetting> gateways, Action<AsyncResult<List<GatewayItem>>> onComplete)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                e.Result = LoadGatewaysInternal(gateways);
            };
            worker.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    onComplete(new AsyncResult<List<GatewayItem>>
                    {
                        Success = false,
                        ErrorMessage = e.Error.Message
                    });
                }
                else
                {
                    var result = (AsyncResult<List<GatewayItem>>)e.Result;
                    onComplete(result);
                }
            };
            worker.RunWorkerAsync();
        }

        private AsyncResult<List<GatewayItem>> LoadGatewaysInternal(List<GatewaySetting> gateways)
        {
            var ar = new AsyncResult<List<GatewayItem>>();

            try
            {
                if (gateways == null || gateways.Count == 0)
                {
                    ar.Success = true;
                    ar.Data = new List<GatewayItem>();
                    return ar;
                }

                var result = new List<GatewayItem>();
                var sync = new object();

                int remaining = gateways.Count;
                var doneEvent = new ManualResetEvent(false);

                // Ограничим параллелизм (иначе при большом списке можно создать бурю ICMP)
                var semaphore = new Semaphore(10, 10);

                foreach (var g in gateways)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        semaphore.WaitOne();
                        try
                        {
                            int rtt = MeasureRttMedian(g.GatewayAddress, attempts: 3, timeoutMs: 1500);

                            var item = new GatewayItem
                            {
                                Key = g.GatewayKey,
                                Name = g.GatewayTittle,
                                Address = g.GatewayAddress,
                                Rtt = rtt // -1 если не удалось
                            };

                            lock (sync)
                            {
                                result.Add(item);
                                _gatewayCache.Add(g.GatewayKey, item);
                            }
                        }
                        finally
                        {
                            semaphore.Release();

                            if (Interlocked.Decrement(ref remaining) == 0)
                                doneEvent.Set();
                        }
                    });
                }

                // ВАЖНО: ждём завершения всех ThreadPool работ ПРЯМО ЗДЕСЬ
                // Так как мы уже на фоне (BackgroundWorker), блокировка не заморозит UI.
                doneEvent.WaitOne();

                // Сортировка: доступные сверху, по RTT
                result.Sort((a, b) =>
                {
                    if (a.Rtt < 0 && b.Rtt < 0) return 0;
                    if (a.Rtt < 0) return 1;
                    if (b.Rtt < 0) return -1;
                    return a.Rtt.CompareTo(b.Rtt);
                });

                ar.Success = true;
                ar.Data = result;
                return ar;
            }
            catch (Exception ex)
            {
                ar.Success = false;
                ar.ErrorMessage = ex.Message;
                ar.Data = new List<GatewayItem>();
                return ar;
            }
        }

        private int MeasureRttMedian(string host, int attempts, int timeoutMs)
        {
            var rtts = new List<long>();

            for (int i = 0; i < attempts; i++)
            {
                try
                {
                    using (var ping = new Ping())
                    {
                        var reply = ping.Send(host, timeoutMs);
                        if (reply != null && reply.Status == IPStatus.Success)
                            rtts.Add(reply.RoundtripTime);
                    }
                }
                catch
                {
                    // игнорируем
                }

                Thread.Sleep(100);
            }

            if (rtts.Count == 0)
                return -1;

            rtts.Sort();
            return (int)rtts[rtts.Count / 2];
        }
    }
}
