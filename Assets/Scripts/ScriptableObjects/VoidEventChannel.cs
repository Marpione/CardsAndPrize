using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidEventChannel : ScriptableObject
{
    public event Action OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
