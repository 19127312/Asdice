using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : Flyweight
{
    [SerializeField]
    private MeshRenderer meshRenderer;

    private void OnEnable()
    {
        TurnOff().Forget();
    }

    public void SetupMat(Material material)
    {
        meshRenderer.material = material;
    }

    public void Despawn()
    {
        FlyweightFactory.ReturnToPool(this);
    }

    private async UniTask TurnOff()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2), ignoreTimeScale: false);
        Despawn();
    }
}
