using UnityEngine;

public class HammerCollision : MonoBehaviour
{
    [SerializeField] private int damage = 10000;
    private void OnTriggerEnter(Collider other)
    {
        Character character =  other.GetComponent<Character>();
        if(character != null)
        {
            character.EarnDamage(damage, gameObject);
        }
    }
}
