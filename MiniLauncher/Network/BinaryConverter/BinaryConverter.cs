using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MiniLauncher.Network.BinaryConverter
{
    public static class BinaryStructConverter
    {
        public static T FromByteArray<T>(byte[] bytes, int start_pos = 0, int sizeT = 0) where T : struct
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                int resSize = 0;

                int size = sizeT == 0 ? Marshal.SizeOf(typeof(T)) : sizeT;

                if (bytes.Length < size)
                {
                    resSize = bytes.Length;
                }
                else
                {
                    resSize = size;
                }

                ptr = Marshal.AllocHGlobal(size);
                Marshal.Copy(bytes, start_pos, ptr, resSize);
                object obj = Marshal.PtrToStructure(ptr, typeof(T));
                return (T)obj;
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }
        }


        public static byte[] ToByteArray<T>(T obj) where T : struct
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                int size = Marshal.SizeOf(typeof(T));
                ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(obj, ptr, true);
                byte[] bytes = new byte[size];
                Marshal.Copy(ptr, bytes, 0, size);
                return bytes;
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
