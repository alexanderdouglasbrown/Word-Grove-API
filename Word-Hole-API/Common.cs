using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Word_Grove_API
{
    public static class Common
    {
        public static string GetPSTTimeString(DateTime time)
        {
            var format = "dddd, MMM dd, yyyy hh:mm tt";
            TimeZoneInfo timeZone;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles");
            }
            else
            {
                return time.ToString(format);
            }

            var adjustedTime = TimeZoneInfo.ConvertTime(time, timeZone);

            return adjustedTime.ToString(format);
        }
    }
}
