using MiniLauncher.Data;
using MiniLauncher.Network.BinaryConverter;
using MiniLauncher.Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MiniLauncher.Network
{
    public class NetworkClient : NetworkEx
    {
        private Default_Set _defaulSet;
        private readonly List<ServerState> _serverList;
        private _login_account_request_cllo _loginAccountRequest;
        private _crypty_key_inform_locl _cryptyKeyInformLocl;

        public NetworkClient(string sIpAddr, int iPort)
            : base(sIpAddr, iPort)
        {
            _defaulSet = new Default_Set();
            // Init ServerState List
            _serverList = new List<ServerState>();

        }
        // Сводка:
        //     Called when connection is established
        public override void AcceptClientCheck(Socket client)
        {
            // Send pack for XOR Login public keys
            CryptoKeyRequest(client);
        }

        public override void DataAnalyze(Socket client, _MSG_HEADER header, byte[] data)
        {
            if (header.m_byType == 21)
            {
                LoginLineAnalyze(client, header, data);
            }
        }
        // Сводка:
        //  Called when new packets arrived
        private bool LoginLineAnalyze(Socket n, _MSG_HEADER pMsgHeader, byte[] pMsg)
        {
            switch ((int)pMsgHeader.m_bySubType)
            {
                case _crypty_key_inform_locl.ID:
                    CryptoKeyResult(n, pMsg);
                    break;
                case _login_account_result_locl.ID:
                    AfterLogin(n, pMsg);
                    break;
                case _push_close_result_locl.ID:
                    PushCloseAccountResult(n, pMsg);
                    break;
                case _world_list_result_locl.ID:
                    WorldListResult(pMsg);
                    break;
                case _world_user_inform_locl.ID:
                    WorldListInform(n, pMsg);
                    break;
                case _select_world_result_locl.ID:
                    SelectWorldResult(n, pMsg);
                    break;
            }
            return true;
        }

        private void CryptoKeyRequest(Socket client)
        {
            _crypty_key_request_cllo cryptyKeyRequest = new _crypty_key_request_cllo();
            cryptyKeyRequest.header.m_wSize = cryptyKeyRequest.size();
            cryptyKeyRequest.header.m_byType = 21;
            cryptyKeyRequest.header.m_bySubType = 12;
            byte[] bCryptyKeyRequest = BinaryStructConverter.ToByteArray(cryptyKeyRequest);

            Send(client, bCryptyKeyRequest);
        }
        void CryptoKeyResult(Socket client, byte[] pMsg)
        {
            _cryptyKeyInformLocl = BinaryStructConverter.FromByteArray<_crypty_key_inform_locl>(pMsg);
            NetworkClientEventArgs args = new NetworkClientEventArgs();
            args.AccountSerial = 0;
            args.Servers = null;
            args.DefaultSet = new Default_Set();
            args.SConnection = client;
            args.CState = NetworkClientEventArgs.Callback.CRYPTO_KEY_INFORM;
            OnEvent(args);
        }
        public void DoLogin(string sLogin, string sPassword)
        {
            _loginAccountRequest = new _login_account_request_cllo();
            // Init Arrays //
            _loginAccountRequest.szAccountID = new byte[13];
            _loginAccountRequest.szPassword = new byte[13];
            _defaulSet.id_account = new byte[13];

            // Fill in login and password bytes //
            var login = Encoding.ASCII.GetBytes(sLogin);
            var password = Encoding.ASCII.GetBytes(sPassword);
            // Fill in DefaultSet
            login.CopyTo(_defaulSet.id_account, 0);
            // Resize to 13 //
            Array.Resize(ref login, 13);
            Array.Resize(ref password, 13);
            // XOR Encrypt //
            EnCryptString(login, 13, _cryptyKeyInformLocl.byPlus, _cryptyKeyInformLocl.wKey);
            EnCryptString(password, 13, _cryptyKeyInformLocl.byPlus, _cryptyKeyInformLocl.wKey);
            // Copy XOR encrypted data to pack //
            login.CopyTo(_loginAccountRequest.szAccountID, 0);
            password.CopyTo(_loginAccountRequest.szPassword, 0);
            // Set server type //
            _loginAccountRequest.byServerType = 0;

            _loginAccountRequest.header.m_wSize = _loginAccountRequest.size();
            _loginAccountRequest.header.m_byType = 21;
            _loginAccountRequest.header.m_bySubType = 3;

            byte[] bLoginAccountRequest = BinaryStructConverter.ToByteArray(_loginAccountRequest);

            Send(Client, bLoginAccountRequest);
        }

        void AfterLogin(Socket client, byte[] pMsg)
        {
            _login_account_result_locl loginAccountResultLocl = BinaryStructConverter.FromByteArray<_login_account_result_locl>(pMsg);
            // Fill in default set //
            _defaulSet.account_serial = loginAccountResultLocl.dwAccountSerial;
            _defaulSet.is_premium = loginAccountResultLocl.dwBillingType > 1 ? 0 : (uint)1;

            // EventArgs for callback
            NetworkClientEventArgs args = new NetworkClientEventArgs();
            args.AccountSerial = loginAccountResultLocl.dwAccountSerial;
            args.Servers = null;
            args.DefaultSet = _defaulSet;
            args.SConnection = client;

            switch (loginAccountResultLocl.byRetCode)
            {
                case 0x00: // OK
                    args.CState = NetworkClientEventArgs.Callback.NULL;
                    WorldListRequest(client);
                    break;
                case 48:   // Server CLOSE
                    args.CState = NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_SERVER_CLOSED;
                    break;
                case 0x05: // Account allready in game
                    args.CState = NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_ALREADY_IN_GAME;
                    break;
                case 0x06: // Login Wrong
                    args.CState = NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_WRONG_LOGIN;
                    break;
                case 0x07: // PW Wrong
                    args.CState = NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_WRONG_PW;
                    break;
                case 0x28: // Account BLOCK
                    args.CState = NetworkClientEventArgs.Callback.LOGIN_ACCOUNT_BANNED;
                    args.ZBlockReason = loginAccountResultLocl.uszBlockReason;
                    StopListen(true);
                    break;
                default:
                    args.CState = NetworkClientEventArgs.Callback.NULL;
                    break;
            }

            OnEvent(args);
        }
        public void CloseAccountRequest(Socket client)
        {
            _push_close_request_cllo pushCloseRequest = new _push_close_request_cllo();

            pushCloseRequest.header.m_wSize = pushCloseRequest.size();
            pushCloseRequest.header.m_byType = 21;
            pushCloseRequest.header.m_bySubType = 9;
            pushCloseRequest.szAccountID = new byte[13];
            pushCloseRequest.szPassword = new byte[13];


            _loginAccountRequest.szAccountID.CopyTo(pushCloseRequest.szAccountID, 0);
            _loginAccountRequest.szPassword.CopyTo(pushCloseRequest.szPassword, 0);
            pushCloseRequest.dwAccountSerial = _defaulSet.account_serial;

            byte[] bPushCloseRequest = BinaryStructConverter.ToByteArray(pushCloseRequest);
            Send(client, bPushCloseRequest);
        }
        public void PushCloseAccountResult(Socket client, byte[] pMsg)
        {
            _push_close_result_locl pushCloseResult = BinaryStructConverter.FromByteArray<_push_close_result_locl>(pMsg);
            if (pushCloseResult.byRetCode == 0)
            {
                CryptoKeyRequest(client);
            }
        }
        void WorldListRequest(Socket client)
        {
            _world_list_request_cllo worldListRequest = new _world_list_request_cllo();

            worldListRequest.header.m_wSize = worldListRequest.size();
            worldListRequest.header.m_byType = 21;
            worldListRequest.header.m_bySubType = 5;
            worldListRequest.dwClientVersion = 0;

            byte[] bWorldListRequest = BinaryStructConverter.ToByteArray(worldListRequest);
            Send(client, bWorldListRequest);
        }

        void WorldListResult(byte[] pMsg)
        {
            _world_list_result_locl worldListResultLocl = BinaryStructConverter.FromByteArray<_world_list_result_locl>(pMsg);

            if (worldListResultLocl.byRetCode == 0)
            {
                int nIndx = 0;
                int nCountWorld = worldListResultLocl.sListData[nIndx++];
                for (int i = 0; i < nCountWorld; ++i)
                {
                    byte bWorldState = worldListResultLocl.sListData[nIndx++];
                    byte byWorldNameLen = worldListResultLocl.sListData[nIndx++];

                    byte[] bName = new byte[byWorldNameLen];
                    for (int j = 0; j < byWorldNameLen; j++)
                    {
                        bName[j] = worldListResultLocl.sListData[j + nIndx];
                    }
                    _serverList.Add(new ServerState
                    {
                        s_ServerName = Encoding.ASCII.GetString(bName).Substring(0, byWorldNameLen - 1),
                        b_ServerState = bWorldState
                    });
                    nIndx += byWorldNameLen + 1;
                }
            }
        }
        private void WorldListInform(Socket client, byte[] pMsg)
        {
            _world_user_inform_locl worldUserInform = BinaryStructConverter.FromByteArray<_world_user_inform_locl>(pMsg);
            for (int i = 0; i < worldUserInform.byServiceWorldNum; i++)
            {
                ServerState state = _serverList[i];
                state.i_ServerLoad = worldUserInform.wUserNum[i];
            }

            NetworkClientEventArgs args = new NetworkClientEventArgs();
            args.Servers = _serverList;
            args.DefaultSet = _defaulSet;
            args.CState = NetworkClientEventArgs.Callback.SERVER_LIST_INFORM;
            args.SConnection = client;
            OnEvent(args);
        }
        public void SelectWordlRequest(int idServer)
        {
            _defaulSet.world_id = (ushort)idServer;
            _select_world_request_cllo selectWorldRequest = new _select_world_request_cllo();
            selectWorldRequest.header.m_wSize = selectWorldRequest.size();
            selectWorldRequest.header.m_byType = 21;
            selectWorldRequest.header.m_bySubType = 7;
            selectWorldRequest.wWorldIndex = (byte)idServer;

            byte[] bSelectWorldRequest = BinaryStructConverter.ToByteArray(selectWorldRequest);
            Send(Client, bSelectWorldRequest);
        }
        private void SelectWorldResult(Socket client, byte[] pMsg)
        {
            _select_world_result_locl selectWorldResult = BinaryStructConverter.FromByteArray<_select_world_result_locl>(pMsg);
            if (selectWorldResult.byRetCode == 0)
            {

                // Fill in last data to defaultSet
                var serverCfg = LauncherConfig.GetInstance.ServerConfig;
                if (serverCfg.OverrideServerAddress)
                {
                    _defaulSet.world_ipv4 = (uint)IPAddress.Parse(serverCfg.ServerAddress.Split(':')[0]).Address;
                    _defaulSet.world_port = ushort.Parse(serverCfg.ServerAddress.Split(':')[1]);
                } else {
                    _defaulSet.world_ipv4 = selectWorldResult.dwWorldGateIP;
                    _defaulSet.world_port = selectWorldResult.wWorldGatePort;
                }

                _defaulSet.world_master_key = new uint[4];
                for (int i = 0; i < 4; i++)
                {
                    _defaulSet.world_master_key[i] = selectWorldResult.dwWorldMasterKey[i];
                }
                _defaulSet.allow_alt_tab = selectWorldResult.bAllowAltTab;
                _defaulSet.reserved = 0;
                var nationCfg = LauncherConfig.GetInstance.NationalConfig;
                if (nationCfg.OverrideNationalCode)
                {
                    _defaulSet.nation_code = (ushort)nationCfg.NationCode;
                }
                else
                {
                    _defaulSet.nation_code = (ushort)e_nation_code.ru_ru;
                }
                StopListen(true);

                NetworkClientEventArgs args = new NetworkClientEventArgs();
                args.Servers = _serverList;
                args.DefaultSet = _defaulSet;
                args.CState = NetworkClientEventArgs.Callback.SERVER_SESSION_RESULT;
                args.SConnection = client;
                OnEvent(args);
            }
        }
        public void StopListen(bool terminate = false)
        {
            if (terminate)
            {
                NetworkClientEventArgs args = new NetworkClientEventArgs();
                args.CState = NetworkClientEventArgs.Callback.NETWORK_CLIENT_STOP;
                OnEvent(args);
            }
            base.StopListen();
        }

        protected virtual void OnEvent(NetworkClientEventArgs e)
        {
            ClientEvents?.Invoke(this, e);
        }

        public event EventHandler<NetworkClientEventArgs> ClientEvents;
    }

    public class NetworkClientEventArgs : EventArgs
    {
        public Default_Set DefaultSet { get; set; }
        public List<ServerState> Servers { get; set; }
        public UInt32 AccountSerial { get; set; }
        public string ZBlockReason { get; set; }
        public Callback CState { get; set; }
        public Socket SConnection { get; set; }
        public enum Callback
        {
            SERVER_LIST_INFORM = 0,
            SERVER_SELECT_WORLD_RESULT = 1,
            LOGIN_ACCOUNT_ALREADY_IN_GAME = 2,
            LOGIN_ACCOUNT_BANNED = 3,
            LOGIN_ACCOUNT_WRONG_LOGIN = 4,
            LOGIN_ACCOUNT_WRONG_PW = 5,
            LOGIN_ACCOUNT_SERVER_CLOSED = 6,
            NETWORK_CLIENT_STOP = 7,
            SERVER_SESSION_RESULT = 8,
            CRYPTO_KEY_INFORM = 9,
            NULL = 10
        }
    }
}
