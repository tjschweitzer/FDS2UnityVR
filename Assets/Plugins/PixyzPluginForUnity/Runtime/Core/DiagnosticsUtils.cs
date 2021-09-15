using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pixyz.Plugin4Unity
{
    internal static class DiagnosticsUtils
    {
        internal static string GetStackAsString(int depth)
        {
            string output = "";

            var stackTrace = new System.Diagnostics.StackTrace();

            for (int i = depth + 1; i > 0; i--)
            {
                MethodBase methodBase = stackTrace.GetFrame(i).GetMethod();
                string typeName = methodBase.DeclaringType.Name;
                string methodName = methodBase.Name;
                if (i < depth + 1)
                    output += ">";
                output += $"{methodBase.DeclaringType.Name}::{methodBase.Name}";
            }
            
            return output;
        }
    }
}
