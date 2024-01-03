

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class DiceFaceCheck : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_Mask;

    private int currentNumber = 0;

    public int CurrentNumber
    {
        get { return currentNumber; }
    }

    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.up *10, Color.green);
        var ray = new Ray(transform.position, Vector3.up);
        if (Physics.Raycast(ray, out RaycastHit hit, 3, m_Mask))
        {
            currentNumber = Int32.Parse(hit.collider.gameObject.name);
        }
    }
}
