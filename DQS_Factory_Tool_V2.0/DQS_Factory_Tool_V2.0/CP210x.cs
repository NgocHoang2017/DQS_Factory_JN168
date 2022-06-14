using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CP210x
{
    public class CP210x
    {
        [DllImport(@"C:\NXP\ProductionFlashProgrammer\LibDll\CP210xManufacturing.dll")]
        private static extern Int32 CP210x_GetNumDevices(ref Int32 numOfDevices);
        public static Int32 GetNumDevices(ref Int32 numOfDevices)
        {
            return CP210x_GetNumDevices(ref numOfDevices);
        }

        [DllImport(@"C:\NXP\ProductionFlashProgrammer\LibDll\CP210xManufacturing.dll")]
        private static extern Int32 CP210x_Open(Int32 deviceNum, ref IntPtr handle);
        public static Int32 Open(Int32 deviceNum, ref IntPtr handle)
        {
            return CP210x_Open(deviceNum, ref handle);
        }

        [DllImport(@"C:\NXP\ProductionFlashProgrammer\LibDll\CP210xManufacturing.dll")]
        private static extern Int32 CP210x_Close(IntPtr handle);
        public static Int32 Close(IntPtr handle)
        {
            return CP210x_Close(handle);
        }

        [DllImport(@"C:\NXP\ProductionFlashProgrammer\LibDll\CP210xManufacturing.dll")]
        private static extern Int32 CP210x_GetPartNumber(IntPtr handle, Byte[] lpbPartNum);
        public static Int32 GetPartNumber(IntPtr handle, Byte[] lpbPartNum)
        {
            return CP210x_GetPartNumber(handle, lpbPartNum);
        }

        [DllImport(@"C:\NXP\ProductionFlashProgrammer\LibDll\CP210xRuntime.dll")]
        private static extern Int32 CP210xRT_WriteLatch(IntPtr handle, UInt16 mask, UInt16 latch);
        public static Int32 WriteLatch(IntPtr handle, UInt16 mask, UInt16 latch)
        {
            return CP210xRT_WriteLatch(handle, mask, latch);
        }

        [DllImport(@"C:\NXP\ProductionFlashProgrammer\LibDll\CP210xRuntime.dll")]
        private static extern Int32 CP210xRT_ReadLatch(IntPtr handle, UInt16[] lpLatch);
        public static Int32 ReadLatch(IntPtr handle, UInt16[] lpLatch)
        {
            return CP210xRT_ReadLatch(handle, lpLatch);
        }

        [DllImport(@"C:\NXP\ProductionFlashProgrammer\LibDll\CP210xManufacturing.dll")]
        private static extern Int32 CP210x_SetProductString(IntPtr handle, StringBuilder Product, ref byte Length, bool ConverttoASCII);
        public static Int32 SetProductString(IntPtr handle, StringBuilder Product, ref byte Length, bool ConverttoASCII)
        {
            return CP210x_SetProductString( handle, Product, ref Length , ConverttoASCII);
        }
        [DllImport(@"C:\NXP\ProductionFlashProgrammer\LibDll\CP210xManufacturing.dll")]
        private static extern Int32 CP210x_GetDeviceProductString(IntPtr handle, StringBuilder Product, ref byte Length, bool ConverttoASCII);
        public static Int32 GetDeviceProductString(IntPtr handle, StringBuilder Product, ref byte Length, bool ConverttoASCII)
        {
            return CP210x_GetDeviceProductString(handle, Product,ref Length, ConverttoASCII);
        }
        [DllImport(@"C:\NXP\ProductionFlashProgrammer\LibDll\CP210xManufacturing.dll")]
        private static extern Int32 CP210x_Reset(IntPtr handle);
        public static Int32 Reset(IntPtr handle)
        {
            return CP210x_Reset(handle);
        }
    }
}


