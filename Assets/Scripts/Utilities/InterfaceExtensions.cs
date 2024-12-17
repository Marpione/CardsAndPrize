using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class InterfaceExtensions
{
    public static T GetComponent<T>(this object obj) where T : class
    {
        if (obj is Component component)
        {
            return component.GetComponent<T>();
        }
        else if (obj is GameObject gameObject)
        {
            return gameObject.GetComponent<T>();
        }
        return null;
    }

    public static T GetInterfaceComponent<T>(this GameObject gameObject) where T : class
    {
        return gameObject.GetComponents<Component>()
            .OfType<T>()
            .FirstOrDefault();
    }
}