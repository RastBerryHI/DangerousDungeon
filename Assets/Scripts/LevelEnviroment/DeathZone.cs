using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Character>().EarnDamage(10000, gameObject);
            return;
        }
        Destroy(other.gameObject);
    }
}
