using UnityEngine;

public class LumberAxe : Artifact
{
    [SerializeField] private Lumberjack lumberjack;
    [SerializeField] private float chance;
    
    private Character owner;

    private void Start()
    {
        LevelGenerator.s_levelGenerator.onEnemyDie.AddListener(SummonHobbit);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Character _owner = other.GetComponent<Character>();
            PlayerPockets _pocket = other.GetComponent<PlayerPockets>();

            if(_owner == null)
            {
                WarnerMessages.NoComponent("Character", other);
                return;
            }
            if(_pocket == null)
            {
                WarnerMessages.NoComponent("PlayerPockets", other);
                return;
            }

            transform.parent = _pocket.ArtifactPocket.transform;
            transform.position = _pocket.ArtifactPocket.transform.position;
            transform.rotation = _pocket.ArtifactPocket.transform.rotation;
            owner = _owner;

            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void SummonHobbit()
    {
        if(owner == null) return;
        if (Random.value <= chance) return;

        GameObject[] entities = GameObject.FindGameObjectsWithTag("Character");
        GameObject _target = entities[Random.Range(0, entities.Length)];

        Lumberjack _lumberjack = Instantiate<Lumberjack>(lumberjack, _target.transform.position, Quaternion.identity);

        System.Array.Clear(entities, 0, entities.Length);
        entities = null;
    }
}
