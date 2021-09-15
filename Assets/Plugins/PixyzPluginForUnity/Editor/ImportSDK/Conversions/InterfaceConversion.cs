using System;

namespace Pixyz.ImportSDK
{
    public static class InterfaceConversion
    {
        #region Time
        public static DateTime ToUnityObject(this Plugin4Unity.Native.Core.Date date)
        {
            return new DateTime(date.year, date.month, date.day);
        }

        public static string ToEndDateRichText(this Plugin4Unity.Native.Core.Date date)
        {
            var validity = new DateTime(date.year, date.month, date.day);
            int daysRemaining = Math.Max(0, (validity - DateTime.Now).Days + 1);
            if (daysRemaining == 0)
                return date.ToUnityObject().ToString("yyyy-MM-dd") + "   <color='red'><i>Expired</i></color>";
            string remainingTextColor = daysRemaining > 30 ? "green" : daysRemaining > 15 ? "orange" : "red";
            return date.ToUnityObject().ToString("yyyy-MM-dd") + "   <color='" + remainingTextColor + "'><i>" + daysRemaining + " remaining day" + (daysRemaining > 1 ? "s" : "") + "</i></color>";
        }

        public static string GetRemainingDaysText(this Plugin4Unity.Native.Core.Date date)
        {
            var validity = new DateTime(date.year, date.month, date.day);
            int daysRemaining = Math.Max(0, (validity - DateTime.Now).Days + 1);
            if (daysRemaining == 0)
                return "<color='red'>Expired</color>";
            string remainingTextColor = daysRemaining > 30 ? "green" : daysRemaining > 15 ? "orange" : "red";
            return "<color='" + remainingTextColor + "'>" + daysRemaining + " Day" + (daysRemaining > 1 ? "s" : "") + "</color>";
        }

        #endregion

        #region Ids
        public static UInt32 ToUInt32(this Int32 i)
        {
            return BitConverter.ToUInt32(BitConverter.GetBytes(i), 0);
        }

        public static Int32 ToInt32(this UInt32 i)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(i), 0);
        }
        #endregion
    }
}

