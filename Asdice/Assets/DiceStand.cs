using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceStand : MonoBehaviour
{
    [SerializeField]
    private GameObject dicePrefab;

    [SerializeField]
    private Transform spawnLocation;

    [SerializeField]
    private List<DiceSettings> dices;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Dice"))
        {
            Debug.Log("Dice is in the stand");
        }
    }
    
    private void SpawnDice(int index)
    {
        var dice = FlyweightFactory.Spawn(dices[index]);
        dice.transform.position = spawnLocation.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnDice(0);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnDice(1);
        }
    }
}
