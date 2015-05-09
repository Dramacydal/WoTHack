using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteMagic;
using WhiteMagic.WinAPI;

namespace WoTHack
{
    public class SniperTreeHackBP2 : HardwareBreakPoint
    {
        public static IntPtr Addr = new IntPtr(0x82E07A - 0x400000);

        public SniperTreeHackBP2()
            : base(Addr, 1, Condition.Code)
        {
        }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Eip += 0x5;
            return true;
        }
    }

    public class NoSniperTreeHackBP : HardwareBreakPoint
    {
        public static IntPtr Addr = new IntPtr(0x82E070 - 0x400000);

        public NoSniperTreeHackBP()
            : base(Addr, 1, Condition.Code)
        {
        }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Eip += 0x2;
            return true;
        }
    }

    public class SniperTreeHackBP1 : HardwareBreakPoint
    {
        public static IntPtr Addr = new IntPtr(0x82E09E - 0x400000);

        public SniperTreeHackBP1()
            : base(Addr, 1, Condition.Code)
        {
        }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Eip += 0x10;

            /*var pAddr = pd.ReadPointer();
            if (printed < 5)
                Console.WriteLine(pd.Read<float>(pAddr));
            //pd.Write<float>(pAddr, 1E+32f);
                
            pAddr = pd.ReadPointer(new IntPtr(ctx.Ebp - 0x94));
            if (printed < 5)
                Console.WriteLine(pd.Read<float>(pAddr));
            //pd.Write<float>(pAddr, 1E+32f);

            */

            // FF800000

            pd.Write<uint>(new IntPtr(ctx.Ebp - 0x8C), 0xFF800000);
            pd.Write<uint>(new IntPtr(ctx.Ebp - 0x94), 0xFF800000);
            return true;
        }
    }

    public class MyHack : HardwareBreakPoint
    {
        public static IntPtr Addr = new IntPtr(0x53C428 - 0x400000);

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Eip += 0x2;

            pd.Write<float>(pd.Process.MainModule.BaseAddress.Add(0x21335E4 - 0x400000), 1000 * 1000);
            pd.Write<float>(pd.Process.MainModule.BaseAddress.Add(0x21335E8 - 0x400000), 1000);
            pd.Write<float>(pd.Process.MainModule.BaseAddress.Add(0x21335EC - 0x400000), 1000);

            return true;
        }
    }
}
