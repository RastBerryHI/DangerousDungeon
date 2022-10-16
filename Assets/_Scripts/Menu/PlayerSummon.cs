using CharacterComponents;
using UnityEngine;

public class PlayerSummon : MonoBehaviour
{
    public static PlayerSummon s_instance;

    [SerializeField] private Transform spawn;
    [SerializeField] private PlayerMovable[] movables;
    [SerializeField] private new PlayerCamera camera;

    private PlayerMovable character;

    private int characterSelected;

    public PlayerMovable Character => character;

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

        if (characterSelected < 1)
        {
            characterSelected = 1;
        }

        character = Instantiate(movables[characterSelected - 1], spawn.position, Quaternion.identity);

        // if (characterSelected == 2)
        // {
        //     Necromancer chell = character as Necromancer;
        //     if (chell != null)
        //     {
        //         chell.Camera = camera.GetComponent<Camera>();
        //         camera.player = chell.transform;
        //     }
        // }
        // else if (characterSelected == 1)
        // {
        //     Knight chell = character as Knight;
        //     if (chell != null)
        //     {
        //         chell.Camera = camera.GetComponent<Camera>();
        //         camera.player = chell.transform;
        //     }
        // }
        
        character.GetComponent<CameraHoldable>().camera = camera.GetComponent<Camera>();
        camera.player = character;
    }
}
