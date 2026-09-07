using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniLauncherStyle.Services.Interfaces
{
    internal interface IGatewayService
    {
        string GetAddress(string gatewayKey);

        void LoadGateways(List<MiniLauncher.Data.GatewaySetting> gateways, Action<AsyncResult<List<Data.GatewayItem>>> onComplete);
    }
}
