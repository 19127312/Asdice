using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Flyweight : MonoBehaviour
{
    protected FlyweightSettings settings; // Intrinsic state

    public FlyweightSettings Settings
    {
        get { return settings; }
        set { settings = value; }
    }
}
