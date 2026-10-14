// I DID NOT MAKE THIS!!!!!
// I REMEMBER STEALING IT FROM SOMEWHERE MANY YEARS AGO BUT I FORGOT WHERE LMAO
// IM SORRY!!!!
// -Jura

using UnityEngine;
using System;

namespace Aaron.Toolkit.Utils
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = true)]
    public sealed class ConditionalHideAttribute : PropertyAttribute
    {
        public string conditionalSourceField;
        public bool showIfTrue;
        public int enumIndex;

        public ConditionalHideAttribute(string boolVariableName, bool showIfTrue)
        {
            conditionalSourceField = boolVariableName;
            this.showIfTrue = showIfTrue;
        }
        
        public ConditionalHideAttribute(string enumVariableName, int enumIndex)
        {
            conditionalSourceField = enumVariableName;
            this.enumIndex = enumIndex;
        }
    }
}
