using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ConvertToNzTimeZone(this DateTime dateTime) {
            //var timeZone = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
            return dateTime;
            //return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Local, TimeZoneInfo.Local);
        }
    }
}
