using UnityEngine;

/// <summary>
/// base class for all curses
/// </summary>
public abstract class Curse : MonoBehaviour
{
    private Character character;

    public Character Character
    {
        get => character;
        set
        {
            character = value;
        }
    }
    private void Start()
    {
        character = GetComponentInParent<Character>();
        if (character == null)
        {
            WarnerMessages.NoComponentInParent("Characer", this);
            return;
        }

        ImposeACurse(character);
    }

    private void OnDestroy()
    {
        if (character == null) return;

        DispelTheCurse(character);
    }
    public abstract void ImposeACurse(Character character);
    public abstract void DispelTheCurse(Character character);
}
