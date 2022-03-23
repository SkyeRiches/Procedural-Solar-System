using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// adds the ability to collapse certain aspects of the editor settings to allow it to be tidier
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideAttributes : PropertyAttribute
{
    public string conditionalSourceField;
    public int enumIndex;

    public ConditionalHideAttributes(string boolVarName)
    {
        conditionalSourceField = boolVarName;
    }

    public ConditionalHideAttributes(string enumVarName, int enumIndex)
    {
        conditionalSourceField= enumVarName;
        this.enumIndex = enumIndex;
    } 
}
