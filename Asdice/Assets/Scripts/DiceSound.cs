using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DiceSound : MonoBehaviour
{
    [SerializeField]
    private DiceAnimation diceAnimation;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip destructionClip;

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
        audioSource.Stop();
        audioSource.PlayOneShot(destructionClip, 1f);
    }

    private void DiceAnimation_OnPressed()
    {
        audioSource.Play();
    }

    private void DiceAnimation_OnFirstPhase()
    {
        audioSource.volume = 0.75f;
    }

    private void DiceAnimation_OnDeselected()
    {
        audioSource.Stop();
    }

    private void DiceAnimation_OnSecondPhase()
    {
        audioSource.volume = 1f;
    }
}
