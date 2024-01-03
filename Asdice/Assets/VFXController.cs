using UnityEngine;
using UnityEngine.VFX;

public class VFXController : MonoBehaviour
{
    [SerializeField]
    private VisualEffect visualEffect;

    [SerializeField]
    private DiceAnimation diceAnimation;

    [SerializeField]
    private float firstRate = 50f;

    [SerializeField]
    private float firstPower = 0.4f;

    [SerializeField]
    private float secondRate = 70f;

    [SerializeField]
    private float secondPower = 1f;

    private void Awake()
    {
        diceAnimation.OnDiceLongPress += DiceAnimation_OnDiceLongPress;
        diceAnimation.OnPressed += DiceAnimation_OnPressed;
        diceAnimation.OnFirstPhase += DiceAnimation_OnFirstPhase;
        diceAnimation.OnSecondPhase += DiceAnimation_OnSecondPhase;
        diceAnimation.OnDeselected += DiceAnimation_OnDeselected;
    }

    private void OnDestroy()
    {
        diceAnimation.OnDiceLongPress -= DiceAnimation_OnDiceLongPress;
        diceAnimation.OnPressed -= DiceAnimation_OnPressed;
        diceAnimation.OnFirstPhase -= DiceAnimation_OnFirstPhase;
        diceAnimation.OnSecondPhase -= DiceAnimation_OnSecondPhase;
        diceAnimation.OnDeselected -= DiceAnimation_OnDeselected;
    }

    private void DiceAnimation_OnDiceLongPress()
    {
        Destroy(gameObject);
    }

    private void DiceAnimation_OnPressed()
    {
        visualEffect.enabled = true;
    }

    private void DiceAnimation_OnFirstPhase()
    {
        visualEffect.SetFloat("Rate", 50f);
        visualEffect.SetFloat("Power", 0.4f);
    }

    private void DiceAnimation_OnDeselected()
    {
        visualEffect.Reinit();
        visualEffect.enabled = false;
    }

    private void DiceAnimation_OnSecondPhase()
    {
        visualEffect.SetFloat("Rate", 70f);
        visualEffect.SetFloat("Power", 1f);
    }
}
