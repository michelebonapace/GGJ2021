using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ButtonAttribute : PropertyAttribute
{
    public readonly string MethodName;

    public ButtonAttribute(string MethodName)
    {
        this.MethodName = MethodName;
    }
}
