using UnityEngine;

public class HealthPoition : Potion
{
    [SerializeField] private int health;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Character character = other.GetComponent<Character>();
            if (character == null)
            {
                WarnerMessages.NoComponent("Character");
                return;
            }

            if (character.Health == character.MaxHealth)
            {
                return;
            }

            character.Heal(character.MaxHealth);
            Destroy(gameObject);
        }
    }
}
