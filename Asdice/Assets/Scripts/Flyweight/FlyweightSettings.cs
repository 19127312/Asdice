using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlyweightSettings : ScriptableObject
{
    [SerializeField]
    private FlyweightType type;

    [SerializeField]
    private GameObject prefab;

    public GameObject Prefab
    {
        get { return prefab; }
    }

    public FlyweightType Type
    {
        get { return type; }
    }

    public abstract Flyweight OnCreate();

    public virtual void OnGet(Flyweight f) => f.gameObject.SetActive(true);
    public virtual void OnRelease(Flyweight f) => f.gameObject.SetActive(false);
    public virtual void OnDestroyPoolObject(Flyweight f) => Destroy(f.gameObject);
}

public enum FlyweightType
{
    Dice20Crystal,
    Dice20Red,
    Projectile
}