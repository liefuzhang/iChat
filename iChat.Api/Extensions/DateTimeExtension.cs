using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ConvertToNzTimeZone(this DateTime dateTime) {
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Utc, TimeZoneInfo.Local);
        }
    }
}
