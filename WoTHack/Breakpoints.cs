using System;
using WhiteMagic;
using WhiteMagic.WinAPI;
using WhiteMagic.WinAPI.Types;

namespace WoTHack
{
    public class TreeRaidusBp : HardwareBreakPoint
    {
        public static IntPtr Addr = new IntPtr(0x53C428 - 0x400000);
        public static IntPtr pSqrRad = new IntPtr(0x21335E4 - 0x400000);
        public static IntPtr pMaxRad = new IntPtr(0x21335E8 - 0x400000);
        public static IntPtr pMinRad = new IntPtr(0x21335EC - 0x400000);


        public static void WriteVals(float min, float max, MemoryHandler m)
        {
            m.Write<float>(m.Process.MainModule.BaseAddress.Add(pSqrRad), min * max);
            m.Write<float>(m.Process.MainModule.BaseAddress.Add(pMaxRad), max);
            m.Write<float>(m.Process.MainModule.BaseAddress.Add(pMinRad), min);
        }

        public TreeRaidusBp()
            : base(Addr, 1, BreakpointCondition.Code)
        {
        }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Eip += 0x2;

            WriteVals(1000, 1000, pd);
            return true;
        }
    }

    public class AlwaysSniperBP : HardwareBreakPoint
    {
        public static IntPtr Addr = new IntPtr(0x53B4CE - 0x400000);
        private static IntPtr enabliHiding = new IntPtr(0x21335E0 - 0x400000);

        public static void WriteVals(bool enable, MemoryHandler m)
        {
            m.WriteByte(m.Process.MainModule.BaseAddress.Add(enabliHiding), enable ? (byte)1 : (byte)0);
        }

        public AlwaysSniperBP()
            : base(Addr, 1, BreakpointCondition.Code)
        {
        }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Eip += 0x3;
            ctx.Al = 1;
            return true;
        }
    }
}
