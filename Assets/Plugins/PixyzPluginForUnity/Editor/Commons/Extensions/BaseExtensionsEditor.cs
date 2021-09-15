using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pixyz.Commons.Extensions.Editor
{
    public static class BaseExtensionsEditor
    {
        /// <summary>	
        /// This function allows the use of custom values for an enum. If the int value is not defined in the enum, the Enum at value 0 is picked (custom).	
        /// If the enum value is changed, the current int value get changed.	
        /// </summary>	
        /// <typeparam name="E">Enum to use. Enum value equal to 0 must be the Custom one</typeparam>	
        /// <param name="prevEnum"></param>	
        /// <param name="currEnum"></param>	
        /// <param name="prefValue"></param>	
        /// <param name="currValue"></param>	
        public static void MatchEnumWithCustomValue<E>(ref E prevEnum, ref E currEnum, ref int prefValue, ref int currValue)
        {
            if ((int)(object)currEnum != 0 && (int)(object)currEnum != (int)(object)prevEnum)
            {
                currValue = (int)(object)currEnum;
            }
            if (currValue != prefValue)
            {
                Array array = Enum.GetValues(typeof(E));
                var intvalues = array.Cast<int>().ToArray();
                int index = Array.IndexOf(intvalues, currValue);
                if (index >= 0)
                {
                    currEnum = (E)array.GetValue(index);
                }
                else
                {
                    currEnum = (E)(object)0;
                }
            }
            prevEnum = currEnum;
            prefValue = currValue;
        }

        public static void MatchEnumWithCustomValue<E, V, P>(ref E currEnum, E prevEnum, ref V value, string nameof, Dictionary<E, P> presets)
        {
            // From Custom to Preset --> we change value	
            if ((int)(object)prevEnum == 0 && (int)(object)currEnum != 0)
            {
                if (presets.ContainsKey(currEnum))
                {
                    value = presets[currEnum].GetFieldValue<V>(nameof);
                }
            }
            // Last Preset and current Preset are the same, but the value is not "valid" --> we change current Preset to Custom	
            else if (currEnum.Equals(prevEnum) && (int)(object)currEnum != 0)
            {
                V presetValue = presets[currEnum].GetFieldValue<V>(nameof);
                if (!presetValue.Equals(value))
                {
                    currEnum = (E)(object)0;
                }
            }
            // From Preset to Preset --> we change value	
            else if ((int)(object)prevEnum != 0 && (int)(object)currEnum != 0)
            {
                if (presets.ContainsKey(currEnum))
                {
                    value = presets[currEnum].GetFieldValue<V>(nameof);
                }
            }
        }
    }
}