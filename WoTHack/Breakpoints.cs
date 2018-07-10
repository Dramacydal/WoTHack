using System;
using WhiteMagic;
using WhiteMagic.Breakpoints;
using WhiteMagic.Pointers;

namespace WoTHack
{
    public class TreeRaidusBp : CodeBreakpoint
    {
        // wg_setTreeHidingRadius
        // .text:007A1CA1 8B EC                          mov     ebp, esp
        public static ModulePointer Addr = new ModulePointer(0x007A1CA1 - 0x400000);
        public static ModulePointer SetTreeRadiuses = new ModulePointer(0x007A1CA0 - 0x400000);

        public static bool Enabled = true;

        public TreeRaidusBp()
            : base(Addr)
        {
        }

        public static void WriteVals(float Min, float Max, MemoryHandler Memory)
        {
            Memory.Call(SetTreeRadiuses, MagicConvention.StdCall, Min, Max);
        }

        // .text:00536238 F3 0F 11 0D EC+                movss   dword_1B134EC, xmm1
        public override bool HandleException(ContextWrapper Wrapper)
        {
            Wrapper.Context.Eip += 2;
            Wrapper.Context.Ebp = Wrapper.Context.Esp;

            var arg1 = new IntPtr(Wrapper.Context.Ebp + 8);
            var arg2 = new IntPtr(Wrapper.Context.Ebp + 0xC);

            if (Enabled)
            {
                Wrapper.Debugger.Write(arg1, 1000.0f);
                Wrapper.Debugger.Write(arg2, 1000.0f);
            }

            return true;
        }
    }

    public class AlwaysSniperBP : CodeBreakpoint
    {
        // wg_enableTreeHiding
        // .text:007A1C85 8A 4D 08                       mov     cl, [ebp+arg_0]
        public static ModulePointer Addr = new ModulePointer(0x007A1C85 - 0x400000);
        public static ModulePointer EnableTreeHiding = new ModulePointer(0x7A1C60 - 0x400000);

        public static bool Enabled = true;

        public AlwaysSniperBP()
            : base(Addr)
        {
        }

        public static void WriteVals(bool Enable, MemoryHandler Memory)
        {
            Memory.Call(EnableTreeHiding, MagicConvention.StdCall, Enable ? 1 : 0);
        }

        public override bool HandleException(ContextWrapper Wrapper)
        {
            Wrapper.Context.Eip += 3;
            Wrapper.Context.Cl = Enabled ? (byte)1 : Wrapper.Debugger.ReadByte(new IntPtr(Wrapper.Context.Ebp + 8));

            return true;
        }
    }
}
