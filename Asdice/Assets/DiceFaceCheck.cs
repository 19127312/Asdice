using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class DiceFaceCheck : MonoBehaviour
{
    [SerializeField]
    private DiceAnimation diceAnimation;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private LayerMask m_Mask;

    [SerializeField]
    private LayerMask groundMask;

    private bool grounded;
    private int currentNumber = 0;
    private bool isDiceStopped = false;
    private float lastYposition;

    public int CurrentNumber
    {
        get { return currentNumber; }
    }

    private void Awake()
    {
        diceAnimation.OnDeselected += DiceAnimation_OnDeselected;
        diceAnimation.OnPressed += DiceAnimation_OnPressedDice;
        lastYposition = transform.position.y;
    }

    private void DiceAnimation_OnPressedDice()
    {
        isDiceStopped = false;
    }

    private void DiceAnimation_OnDeselected()
    {
        isDiceStopped = true;
    }

    private void OnDestroy()
    {
        diceAnimation.OnDeselected -= DiceAnimation_OnDeselected;
        diceAnimation.OnPressed -= DiceAnimation_OnPressedDice;
    }

    private void CheckDiceFace()
    {
        Debug.DrawRay(transform.position, Vector3.up * 10, Color.green);
        var ray = new Ray(transform.position, Vector3.up);
        if (Physics.Raycast(ray, out RaycastHit hit, 3, m_Mask))
        {
            currentNumber = int.Parse(hit.collider.gameObject.name);
            Debug.Log(currentNumber);
            isDiceStopped = false;
        }
    }

    void Update()
    {
        if (!isDiceStopped) return;
        grounded = Physics.Raycast(transform.position, Vector3.down, 1f, groundMask);

        if (rb.velocity.magnitude == 0 && rb.angularVelocity == Vector3.zero && grounded)
        {
            CheckDiceFace();
        }
    }
}
