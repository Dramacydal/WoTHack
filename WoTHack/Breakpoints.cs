using WhiteMagic;
using WhiteMagic.Breakpoints;
using WhiteMagic.Pointers;

namespace WoTHack
{
    public class TreeRaidusBp : CodeBreakpoint
    {
        // wg_setTreeHidingRadius
        public static ModulePointer Addr = new ModulePointer(0x00536238 - 0x400000);
        public static ModulePointer pSqrRad = new ModulePointer(0x1B134EC - 0x400000);
        public static ModulePointer pMaxRad = new ModulePointer(0x1B134F0 - 0x400000);
        public static ModulePointer pMinRad = new ModulePointer(0x1B134F4 - 0x400000);

        public static void WriteVals(float min, float max, MemoryHandler m)
        {
            m.Write(pSqrRad, min * max);
            m.Write(pMaxRad, max);
            m.Write(pMinRad, min);
        }

        public TreeRaidusBp()
            : base(Addr)
        {
        }

        // .text:00536238 F3 0F 11 0D EC+                movss   dword_1B134EC, xmm1
        public override bool HandleException(ContextWrapper Wrapper)
        {
            Wrapper.Context.Eip += 8;

            WriteVals(1000, 1000, Wrapper.Debugger);
            return true;
        }
    }

    public class AlwaysSniperBP : CodeBreakpoint
    {
        public static ModulePointer Addr = new ModulePointer(0x00534BCE - 0x400000);
        // wg_enableTreeHiding
        private static ModulePointer enableHiding = new ModulePointer(0x1B134E8 - 0x400000);

        public static void WriteVals(bool enable, MemoryHandler m)
        {
            m.Write(enableHiding, enable ? (byte)1 : (byte)0);
        }

        public AlwaysSniperBP()
            : base(Addr)
        {
        }

        // .text:00534BCE 8A 45 FF                       mov     al, [ebp+var_1
        public override bool HandleException(ContextWrapper Wrapper)
        {
            Wrapper.Context.Eip += 3;
            Wrapper.Context.Al = 1;
            return true;
        }
    }
}
