using UnityEngine;

public class PlayerSummon : MonoBehaviour
{
    public static PlayerSummon s_instance;

    [SerializeField] private Transform spawn;
    [SerializeField] private Character[] characters;
    [SerializeField] private new PlayerCamera camera;

    private Character character;

    private int characterSelected;

    public Character Character => character;

    private void Awake()
    {
        characterSelected = PlayerPrefs.GetInt("Character");
    }

    private void Start()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(s_instance.gameObject);
        }

        if (characterSelected < 1) characterSelected = 1;
        character = Instantiate(characters[characterSelected - 1], spawn.position, Quaternion.identity);

        if (characterSelected == 2)
        {
            Necromancer chell = character as Necromancer;
            if (chell != null)
            {
                chell.Camera = camera.GetComponent<Camera>();
                camera.player = chell.transform;
            }
        }
        else if (characterSelected == 1)
        {
            Knight chell = character as Knight;
            if (chell != null)
            {
                chell.Camera = camera.GetComponent<Camera>();
                camera.player = chell.transform;
            }
        }
    }
}
