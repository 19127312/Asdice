using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DiceAnimation : MonoBehaviour
{
    public float rotationSpeed = 30f; // Degrees per second

    [SerializeField]
    private float initialAngular = 10;

    [SerializeField]
    private float timeToRotate = 5f;

    [SerializeField]
    private float firstPhaseSpeedModifier = 0.8f;

    [SerializeField]
    private float secondPhaseSpeedModifier = 0.9f;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Destructible destructible;

    private CancellationTokenSource cancellationTokenSource;
    private float currentTime = 0f;
    private float firstPhaseTime = 0;
    private float secondPhaseTime = 0;
    private bool isFirstPhase = false;
    private bool isSecondPhase = false;

    //Event for long press
    public event Action OnDiceLongPress;

    private void Awake()
    {
        firstPhaseTime = timeToRotate / 5f;
        secondPhaseTime = timeToRotate / 2f;
    }

    /// <summary>
    /// On Pressed Dice.
    /// </summary>
    public void OnPressedDice()
    {
        cancellationTokenSource = new CancellationTokenSource();
        rb.maxAngularVelocity = initialAngular;
        rb.AddTorque(RandomVector3() * rotationSpeed, ForceMode.Impulse);
        UpdateTimer(cancellationTokenSource.Token).Forget();
    }

    private async UniTaskVoid UpdateTimer(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            currentTime += Time.deltaTime;
            await UniTask.Yield();
            if (currentTime >= timeToRotate)
            {
                OnDeselectDice();
                destructible.SelfExplode();
            }
            else if (currentTime >= secondPhaseTime && !isSecondPhase)
            {
                rb.maxAngularVelocity = secondPhaseSpeedModifier;
                rb.AddTorque(rotationSpeed * secondPhaseSpeedModifier * RandomVector3(), ForceMode.Impulse);
                isSecondPhase = true;
            }
            else if (currentTime >= firstPhaseTime && !isFirstPhase)
            {
                rb.maxAngularVelocity = firstPhaseSpeedModifier;
                rb.AddTorque(firstPhaseSpeedModifier * rotationSpeed * RandomVector3(), ForceMode.Impulse);
                isFirstPhase = true;
            }
        }
    }

    private Vector3 RandomVector3()
    {
        return new Vector3(UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f));
    }

    /// <summary>
    /// On Deselected Dice.
    /// </summary>
    public void OnDeselectDice()
    {
        cancellationTokenSource.Cancel();
        currentTime = 0f;
        if (rb != null)
        {
            rb.angularVelocity = Vector3.zero;
        }
        isSecondPhase = false;
        isFirstPhase = false;
    }
}
