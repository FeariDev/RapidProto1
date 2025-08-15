using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : PropertyAttribute
{
    public string Label { get; }

    public ButtonAttribute(string newLabel = null)
    {
        Label = newLabel;
    }
}
