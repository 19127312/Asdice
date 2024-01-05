using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Destructible : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer objectRenderer;

    [SerializeField]
    private Collider objectCollider;

    [SerializeField]
    private Rigidbody body;

    [SerializeField]
    private GameObject brokenPrefab;

    [SerializeField]
    private Shockwave shockwavePrefab;

    [SerializeField]
    private float force = 1000f;

    [SerializeField]
    private float radius = 5f;

    [SerializeField]
    private float fadeDelay = 2f;

    [SerializeField]
    private DiceAnimation diceAnimation;

    private void Awake()
    {
        diceAnimation.OnDiceLongPress += SelfExplode;
    }

    private void OnDestroy()
    {
        diceAnimation.OnDiceLongPress -= SelfExplode;
    }

    //Explode
    public void SelfExplode()
    {
        Destroy(body);

        objectRenderer.enabled = false;
        objectCollider.enabled = false;

        GameObject brokenObject = Instantiate(brokenPrefab, transform.position, transform.rotation); ;
        Shockwave shockwave = Instantiate(shockwavePrefab, transform.position, new Quaternion(0,0,0,0));
        shockwave.Blast().Forget();

        Rigidbody[] rigidbodies = brokenObject.GetComponentsInChildren<Rigidbody>();

        if (transform.childCount > 0)
        {
            Transform[] transformChildren = gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform transformChild in transformChildren)
            {
                if (transformChild != transform)
                    Destroy(transformChild.gameObject);
            }
        }
        
        foreach (Rigidbody childBody in rigidbodies)
        {
            childBody.velocity = body.velocity;
            childBody.AddExplosionForce(force, transform.position, radius);
        }

        FadeOutRigidBodies(rigidbodies, brokenObject, shockwave).Forget();
    }

    private async UniTask FadeOutRigidBodies(Rigidbody[] rigidbodies, GameObject brokenObject, Shockwave shockwave)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(fadeDelay), ignoreTimeScale: false);

        foreach (Rigidbody childBody in rigidbodies)
        {
            Destroy(childBody.gameObject);
        }

        Destroy(brokenObject);
        Destroy(gameObject);
        Destroy(shockwave.gameObject);
    }
}
