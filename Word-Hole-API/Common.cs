using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Word_Hole_API
{
    public static class Common
    {
        public static string GetPSTTimeString(DateTime time)
        {
            var format = "dddd, MMM dd, yyyy hh:mm tt";
            var adjustedTime = TimeZoneInfo.ConvertTime(time, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));

            return adjustedTime.ToString(format);
        }
    }
}
