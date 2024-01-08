using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(menuName = "Flyweight/Projectile Settings")]
public class ProjectileSettings : FlyweightSettings
{
    public float despawnDelay = 5f;
    public float speed = 10f;
    public float damage = 10f;

    public override Flyweight OnCreate()
    {
        var go = Instantiate(Prefab);
        go.SetActive(false);
        go.name = Prefab.name;

        var flyweight = go.GetOrAdd<Projectile>();
        flyweight.Settings = this;

        return flyweight;
    }
}