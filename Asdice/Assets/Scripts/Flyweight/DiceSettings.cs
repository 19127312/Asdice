using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flyweight/Dice Settings")]
public class DiceSettings : FlyweightSettings
{
    [SerializeField]
    private Material diceMaterial;

    public Material DiceMaterial => diceMaterial;

    public override Flyweight OnCreate()
    {
        var go = Instantiate(Prefab);
        var dice = go.GetOrAdd<Dice>();
        dice.Settings = this;
        dice.SetupMat(diceMaterial);

        return dice;
    }
}
