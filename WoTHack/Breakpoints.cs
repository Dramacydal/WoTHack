﻿using System;
using WhiteMagic;
using WhiteMagic.WinAPI;
using WhiteMagic.WinAPI.Types;

namespace WoTHack
{
    public class TreeRaidusBp : HardwareBreakPoint
    {
        // wg_setTreeHidingRadius
        public static IntPtr Addr = new IntPtr(0x525CA8 - 0x400000);
        public static IntPtr pSqrRad = new IntPtr(0x160A7FC - 0x400000);
        public static IntPtr pMaxRad = new IntPtr(0x160A800 - 0x400000);
        public static IntPtr pMinRad = new IntPtr(0x160A804 - 0x400000);

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

        // .text:00525CA8                 movss   dword_160A7FC, xmm1
        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Eip += 0x2;

            WriteVals(1000, 1000, pd);
            return true;
        }
    }

    public class AlwaysSniperBP : HardwareBreakPoint
    {
        public static IntPtr Addr = new IntPtr(0x524D4E - 0x400000);
        // wg_enableTreeHiding
        private static IntPtr enableHiding = new IntPtr(0x160A7DA - 0x400000);

        public static void WriteVals(bool enable, MemoryHandler m)
        {
            m.WriteByte(m.Process.MainModule.BaseAddress.Add(enableHiding), enable ? (byte)1 : (byte)0);
        }

        public AlwaysSniperBP()
            : base(Addr, 1, BreakpointCondition.Code)
        {
        }

        // .text:00524D4E                 mov     al, [ebp+var_1]
        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Eip += 0x51 - 0x4E;
            ctx.Al = 1;
            return true;
        }
    }
}
