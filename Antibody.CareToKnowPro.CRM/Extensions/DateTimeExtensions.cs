using System;

namespace Antibody.CareToKnowPro.CRM.Extensions
{
    public static  class DateTimeExtensions
    {
        public static long ToUnixTimestamp(this DateTime d)
        {
            DateTime d1 = new DateTime(d.Ticks, DateTimeKind.Utc);
            var epoch = d - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)epoch.TotalSeconds;
        }

        public static DateTime FromUnixTimestamp(this long ts)
        {
            DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            d = d.AddSeconds(ts);
            return d;
        }

        /// <summary>
        /// Round the specified date/time to the timespan
        /// i.e. dt.RoundUp(TimeSpan.FromMinutes(15)); will round up to the nearest 15-minute interval    
        /// </summary>
        /// <param name="d"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static DateTime RoundUp(this DateTime d, TimeSpan ts)
        {
            return new DateTime((d.Ticks + ts.Ticks - 1) / ts.Ticks * ts.Ticks, d.Kind);
        }
    }
}
