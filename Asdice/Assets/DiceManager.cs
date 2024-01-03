using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    [SerializeField] 
    private Rigidbody rb;

    [SerializeField]
    private DiceFaceCheck diceCheck;

    private void Update()
    {
        if (rb.velocity.magnitude == 0 && rb.angularVelocity == Vector3.zero)
        {
            Debug.Log(diceCheck.CurrentNumber);
        }
    }
}
