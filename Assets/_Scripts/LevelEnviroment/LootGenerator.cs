using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootGenerator : MonoBehaviour
{
    [SerializeField] private HealthPoition healthPoition;
    [SerializeField] private Transform[] healthPoitionPositions;

    private void Start()
    {
        int chance = Random.Range(0, 3);
        Debug.Log(chance);

        switch (chance)
        {
            case 0:
                Debug.Log("HEALTH POITION");
                Instantiate(healthPoition, healthPoitionPositions[Random.Range(0, healthPoitionPositions.Length)].position, Quaternion.identity);
                break;
        }
    }
}
