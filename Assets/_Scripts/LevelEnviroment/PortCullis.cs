using UnityEngine;

public class PortCullis : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Character")
        {
            Character character = other.GetComponent<Character>();
            if(character != null)
            {
                character.EarnDamage(1000000, gameObject);
            }
            GetComponent<AudioSource>().Stop();
        }
    }
}
