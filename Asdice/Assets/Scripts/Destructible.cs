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
    private float force = 1000f;

    [SerializeField]
    private float radius = 5f;

    [SerializeField]
    private float fadeSpeed = 0.25f;

    [SerializeField]
    private float fadeDelay = 2f;

    [SerializeField]
    private float PieceSleepDelay = 0.5f;

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

        FadeOutRigidBodies(rigidbodies, brokenObject).Forget();
    }

    private async UniTask FadeOutRigidBodies(Rigidbody[] rigidbodies, GameObject brokenObject)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(PieceSleepDelay), ignoreTimeScale: false);
        int activeRigidBodies = rigidbodies.Length;

        while (activeRigidBodies > 0)
        {
            await UniTask.Yield();
            foreach (Rigidbody childBody in rigidbodies)
            {
                if (childBody.IsSleeping())
                {
                    activeRigidBodies--;
                }
            }
        }

        await UniTask.Delay(TimeSpan.FromSeconds(fadeDelay), ignoreTimeScale: false);
        float time = 0f;
        Renderer[] renderers = Array.ConvertAll(rigidbodies, GetRenderersFromRigidBodies);

        foreach (Rigidbody childBody in rigidbodies)
        {
            Destroy(childBody.GetComponent<Collider>());
            Destroy(childBody);
        }

        while (time < 1)
        {
            float step = Time.deltaTime * fadeSpeed;

            foreach (Renderer renderer in renderers)
            {
                if (renderer != null)
                {
                    renderer.transform.Translate(Vector3.down * (step / renderer.bounds.size.y), Space.World);
                }
            }

            time += step;
            await UniTask.Yield();
        }

        foreach (Renderer renderer in renderers)
        {
            Destroy(renderer.gameObject);
        }

        Destroy(brokenObject);
        Destroy(gameObject);
    }


    private Renderer GetRenderersFromRigidBodies(Rigidbody rid)
    {
        return rid.GetComponent<Renderer>();
    }
}
