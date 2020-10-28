using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToTimeZoneDateTime(this DateTime dateTime, string zoneId)
        {
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(zoneId));
        }
    }
}
