using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZRN.UtilsZRN
{
    public static class FuncoesDatas
    {

        public static DateTime ConvertDataHoraTimeZoneUTCtoBrasil(DateTime data)
        {
            TimeZoneInfo localZone = TimeZoneInfo.Local;
            var QtdeHorasCalculo = localZone.GetUtcOffset(DateTime.Now);
            DateTime dt = data.AddHours(QtdeHorasCalculo.Hours);
            return dt;
        }

        public static DateTime ConvertDataHoraTimeZoneBrasiltoUTC(DateTime data)
        {
            TimeZoneInfo localZone = TimeZoneInfo.Local;
            var QtdeHorasCalculo = localZone.GetUtcOffset(DateTime.Now);
            DateTime dt = data.AddHours(QtdeHorasCalculo.Hours * -1);
            return dt;
        }

    }
}