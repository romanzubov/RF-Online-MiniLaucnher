using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MiniLauncher.Network.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi), Serializable]
    public struct _MSG_HEADER
    {
        public UInt16 m_wSize;
        public byte m_byType;
        public byte m_bySubType;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi), Serializable]
    struct _crypty_key_request_cllo
    {
        public _MSG_HEADER header;
        private byte NULL;
        public static int GetId() { return 12; }
        public UInt16 size() { return (UInt16)Marshal.SizeOf(this); }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi), Serializable]
    struct _crypty_key_inform_locl
    {
        public byte byPlus;
        public byte wKey;

        public UInt16 size() { return (UInt16)Marshal.SizeOf(this); }
        public const int ID = 13;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi), Serializable]
    struct _login_account_request_cllo
    {
        public _MSG_HEADER header;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] szAccountID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] szPassword;

        public byte byServerType;

        public UInt16 size() { return (UInt16)Marshal.SizeOf(this); }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi), Serializable]
    struct _login_account_result_locl
    {
        public byte byRetCode;
        public UInt32 dwAccountSerial;
        public UInt32 dwBillingType;
        public char byBeChangedPass;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] nNewAgree;
        [MarshalAs(UnmanagedType.I1)]
        public bool bAdult;
        public UInt32 dwTime;
        [MarshalAs(UnmanagedType.I1)]
        public bool bMOTPEntry;
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string uszBlockReason;
        public UInt16 size() { return (UInt16)Marshal.SizeOf(this); }
        public const int ID = 4;
    };
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi), Serializable]
    struct _push_close_request_cllo
    {
        public _MSG_HEADER header;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] szAccountID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] szPassword;
        public uint dwAccountSerial;

        public UInt16 size() { return (UInt16)Marshal.SizeOf(this); }
    };
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi), Serializable]
    struct _push_close_result_locl
    {
        public byte byRetCode;
        public const int ID = 10;
    };
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi), Serializable]
    struct _world_list_request_cllo
    {
        public _MSG_HEADER header;
        public uint dwClientVersion;
        public UInt16 size() { return (UInt16)Marshal.SizeOf(this); }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi), Serializable]
    struct _world_list_result_locl
    {

        public byte byRetCode;

        public UInt16 wDataSize;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4095)]

        public byte[] sListData;

        public UInt16 size() { return (UInt16)Marshal.SizeOf(this); }
        public const int ID = 6;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi), Serializable]
    struct _world_user_inform_locl
    {

        public byte byServiceWorldNum;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 40)]
        public UInt16[] wUserNum;

        public const int ID = 66;
    };
    public struct ServerState
    {
        public string s_ServerName;
        public byte b_ServerState;
        public UInt16 i_ServerLoad;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi), Serializable]
    struct _select_world_request_cllo
    {
        public _MSG_HEADER header;
        public ushort wWorldIndex;
        public UInt16 size() { return (UInt16)Marshal.SizeOf(this); }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi), Serializable]
    struct _select_world_result_locl
    {
        public byte byRetCode;
        public uint dwWorldGateIP;
        public ushort wWorldGatePort;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] dwWorldMasterKey;
        public byte bAllowAltTab;

        public const int ID = 8;
    };
    public enum e_nation_code
    {
        ko_kr = 0,
        pt_br = 1,
        zn_cn = 2,
        en_gb = 3,
        en_id = 4,
        ja_jp = 5,
        en_ph = 6,
        ru_ru = 7,
        zh_tw = 8,
        es_es = 9,
        th_th = 10,
        tr_tr = 11,
        NUM
    };
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi), Serializable]
    public struct Default_Set
    {
        public UInt32 world_ipv4;
        public UInt16 world_port;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] id_account;
        public UInt32 account_serial;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public UInt32[] world_master_key;
        public UInt16 world_id;
        public UInt32 allow_alt_tab;
        public UInt32 is_premium;
        public UInt32 reserved;
        public UInt16 nation_code;
        public void encrypt()
        {
            world_ipv4 ^= 0xCB9C4B3A;
            world_port ^= 0x4FB6;
            account_serial ^= 0x6E65E0AF;
            world_master_key[0] ^= 0xCFCF22E6;
            world_master_key[1] ^= 0x5BBCDE6F;
            world_master_key[2] ^= 0xACDF5EDA;
            world_master_key[3] ^= 0xBCCD1B37;
            world_id ^= 0x4B3A;
            allow_alt_tab = account_serial ^ (allow_alt_tab == 1 ? 0xC89C183A : 0xD29C283B);
            is_premium ^= 0xC89C183A;
            nation_code ^= 0x32D7;
        }
    }
}
