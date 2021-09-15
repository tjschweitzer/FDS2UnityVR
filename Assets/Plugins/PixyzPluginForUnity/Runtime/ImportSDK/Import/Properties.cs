using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixyz.ImportSDK;
using System;
using System.Linq;

namespace Pixyz.ImportSDK
{
    [System.Serializable]
    public class Properties
    {
        public string[] names;
        public string[] values;

        public Properties()
        {

        }
        public Properties(string[] names, string[] values)
        {
            this.names = names;
            this.values = values;
        }
    }
}

