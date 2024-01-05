using Cysharp.Threading.Tasks;
using Lean.Common;
using System;
using System.Threading;
using UnityEngine;

public class DiceAnimation : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 30f; // Degrees per second

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
    private LeanSelectable lean;

    private CancellationTokenSource cancellationTokenSource;
    private float currentTime = 0f;
    private float firstPhaseTime = 0;
    private float secondPhaseTime = 0;
    private bool isFirstPhase = false;
    private bool isSecondPhase = false;
    private bool isAbleToPress = true;

    public bool IsAbleToPress
    {
        set 
        { 
            isAbleToPress = value; 
        }
    }

    public bool SetDiceLean
    {
        set
        {
            lean.enabled = value;
        }
    }

    //Event for long press
    public event Action OnDiceLongPress;

    //Event for long press
    public event Action OnPressed;

    //Event for long press
    public event Action OnFirstPhase;

    //Event for long press
    public event Action OnSecondPhase;

    //Event for long press
    public event Action OnDeselected;

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
        if (!isAbleToPress) return;
        isAbleToPress = false;

        OnPressed?.Invoke();

        transform.position += new Vector3 (0, 1.5f, 0);
        rb.useGravity = false;
        rb.maxAngularVelocity = initialAngular;
        rb.AddTorque(RandomVector3() * rotationSpeed, ForceMode.Impulse);

        cancellationTokenSource = new CancellationTokenSource();
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
                OnDiceLongPress?.Invoke();
            }
            else if (currentTime >= secondPhaseTime && !isSecondPhase)
            {
                rb.maxAngularVelocity = secondPhaseSpeedModifier;
                rb.AddTorque(rotationSpeed * secondPhaseSpeedModifier * RandomVector3(), ForceMode.Impulse);
                isSecondPhase = true;
                OnSecondPhase?.Invoke();
            }
            else if (currentTime >= firstPhaseTime && !isFirstPhase)
            {
                rb.maxAngularVelocity = firstPhaseSpeedModifier;
                rb.AddTorque(firstPhaseSpeedModifier * rotationSpeed * RandomVector3(), ForceMode.Impulse);
                isFirstPhase = true;
                OnFirstPhase?.Invoke();
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
        OnDeselected?.Invoke();
        cancellationTokenSource.Cancel();
        currentTime = 0f;
        isSecondPhase = false;
        isFirstPhase = false;
        lean.enabled = false;

        if (rb != null)
        {
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = true;   
        }
    }
}
