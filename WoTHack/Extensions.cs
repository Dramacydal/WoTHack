using System;

namespace WoTHack
{
    public static class Extensions
    {
        public static long MSecToNow(this DateTime date)
        {
            return (DateTime.Now.Ticks - date.Ticks) / TimeSpan.TicksPerMillisecond;
        }
    }
}
