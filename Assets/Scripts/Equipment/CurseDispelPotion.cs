using UnityEngine;

public class CurseDispelPotion : Potion
{

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

            Curse[] curses = character.GetComponentsInChildren<Curse>();
            if(curses.Length == 0)
            {
                curses = null;
                return;
            }

            for(int i = 0; i < curses.Length; i++)
            {
                Destroy(curses[i].gameObject);
            }
            System.Array.Clear(curses, 0, curses.Length);
            curses = null;
            Destroy(gameObject);
        }
    }
}
