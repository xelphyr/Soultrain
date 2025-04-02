using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class SerializeReferenceDropdownAttribute : PropertyAttribute
{
    // This attribute marks the field for a custom property drawer.
}