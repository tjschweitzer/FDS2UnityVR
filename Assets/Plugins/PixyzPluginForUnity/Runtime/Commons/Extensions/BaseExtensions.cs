using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pixyz.Commons.Extensions
{
    public static class BaseExtensions
    {
        public static uint ToUInt32(this int i)
        {
            return BitConverter.ToUInt32(BitConverter.GetBytes(i), 0);
        }
        public static string FormatNicely(this TimeSpan time)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", time.Hours, time.Minutes, time.Seconds, time.Milliseconds);
        }
        public static void LogColor(Color color, object message)
        {
            Debug.Log("<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + message + "</color>");
        }
        /// <summary>	
        /// Swaps values or references between two items.	
        /// </summary>	
        /// <typeparam name="T">Items type</typeparam>	
        /// <param name="A">First item</param>	
        /// <param name="B">Second item</param>	
        public static void Swap<T>(ref T A, ref T B)
        {
            T tmp = A;
            A = B;
            B = tmp;
        }
        /// <summary>	
        /// Transforms a camelCase string into a fancy string.	
        /// </summary>	
        /// <param name="str"></param>	
        /// <returns></returns>	
        public static string FancifyCamelCase(this string str)
        {
            if (str.Length > 1 && str[0] == '_') str = str.Substring(1);
            str = str.Replace('_', ' ');
            str = str.Replace("@", "");
            if (str == null) return str;
            if (str.Length < 2) return str.ToUpper();
            var splits = str.Split('.');
            str = splits[splits.Length - 1];
            string result = str.Substring(0, 1).ToUpper();
            for (int i = 1; i < str.Length; i++)
            {
                if (char.IsUpper(str[i]) && !char.IsUpper(str[i - 1]) && str[i - 1] != ' ')
                    result += ' ';
                result += (str[i - 1] == ' ') ? char.ToUpper(str[i]) : char.ToLower(str[i]);
            }
            return result;
        }

        /// <summary>	
        /// Add key and value pair to a dictionary if key wasn't present or get existing value for corresponding key otherwise.	
        /// </summary>	
        /// <typeparam name="K"></typeparam>	
        /// <typeparam name="V"></typeparam>	
        /// <param name="dictionary"></param>	
        /// <param name="key">Dictionary key to look for</param>	
        /// <param name="defaultValue">Value to set if key wasn't present in the dictionary</param>	
        /// <returns></returns>	
        public static V AddOrGet<K, V>(this Dictionary<K, V> dictionary, K key, V defaultValue)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            dictionary.Add(key, defaultValue);
            return defaultValue;
        }
    }
}