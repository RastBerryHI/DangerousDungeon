using System.Collections.Generic;
using CharacterComponents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
enum GameModes : sbyte
{
    Clearing,
    Waves
}

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator s_levelGenerator;
    private int unitCounter; 

    [SerializeField] private GameModes gameMode;
    [SerializeField] private GameObject dungeonUnit;
    [SerializeField] private GameObject[] unitsToLoad;
    [SerializeField] private Vector2 offset;
    [SerializeField] private float xExtraOffset;
    [SerializeField] private int currentEnemyAmount;

    [SerializeField] private int minWaves;
    [SerializeField] private int maxWaves;
    [SerializeField] private int waves;
    [SerializeField] private int unitsTillBoss;
    [SerializeField] private float respawnWave;

    public List<GameObject> allUnits = new List<GameObject>();
    public AIMovable[] enemies;
    public VisualEffect spawnFX;
    public UnityEvent onUnitCleared;
    public UnityEvent onEnemyDie;
    public UnityEvent onWaveCleared;
    public event UnityAction<int> onWavesSet;
    public GameObject DungeonUnit => dungeonUnit;

    public int EnemyAmount => currentEnemyAmount;
    public int UnitCounter => unitCounter;
    public int MinWaves => minWaves;
    public int MaxWaves => maxWaves;
    public int Waves => waves;
    public float RespawnWave => respawnWave;
    private void Awake()
    {
        if (s_levelGenerator == null)
        {
            s_levelGenerator = this;
        }
        else
        {
            Destroy(s_levelGenerator.gameObject);
        }
    }

    private void Start()
    {
        onUnitCleared.AddListener(LoadUnit);
        if (dungeonUnit == null)
        {
            WarnerMessages.UnassingedField("dungeonUnit");
            return;
        }
        if (unitsToLoad.Length == 0)
        {
            WarnerMessages.UnassingedField("unitsToLoad");
            return;
        }

        allUnits.Add(dungeonUnit);
        SpawnWithChance(dungeonUnit);
        DefineGamemode();
    }

    private void DefineGamemode()
    {
        if (dungeonUnit.name.Contains("Elevator"))
        {
            gameMode = GameModes.Waves;
            waves = Random.Range(minWaves, maxWaves + 1);

            if (onWavesSet != null)
            {
                onWavesSet(waves);
            }
        }
        else
        {
            gameMode = GameModes.Clearing;
        }
    }

    public void DecrementEnemyCount(GameObject sender)
    {
        onEnemyDie.Invoke();

        if (dungeonUnit.name.Contains("BossArena"))
        {
            Debug.Log("Boss");
            // TODO: Implement summoned skeletons decrementing
            if (sender.name.Contains("Boss"))
            {
                currentEnemyAmount--;
                if (currentEnemyAmount == 0)
                {
                    onUnitCleared.Invoke();
                }
            }
            return;
        }

        currentEnemyAmount--;
        if (currentEnemyAmount == 0 && gameMode == GameModes.Clearing)
        {
            onUnitCleared.Invoke();
        }
        else if (currentEnemyAmount == 0 && gameMode == GameModes.Waves)
        {
            waves--;
            if (waves > 0 && EnemyAmount == 0)
            {
                onWaveCleared.Invoke();
                SpawnWithChance();
            }
            else
            {
                onUnitCleared.Invoke();
            }
        }
    }

    public void SetCurrentDungeonUnit(GameObject _unitId)
    {
        dungeonUnit = _unitId;
        allUnits.Add(dungeonUnit);

        if (allUnits.Count == 5)
        {
            Destroy(allUnits[0]);
            allUnits.RemoveAt(0);
        }
    }

    public void SetEnemyAmount(int count)
    {
        currentEnemyAmount = count;
    }

    public void SpawnWithChance(GameObject dungeonRoom)
    {
        Transform[] spawnPoints = new Transform[0];
        int counter = 0;

        foreach (Transform child in dungeonRoom.transform)
        {
            if (child.name == "SpawnPoints")
            {
                spawnPoints = child.GetComponentsInChildren<Transform>();
                break;
            }
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawnpoints in dungeon");
            return;
        }

        for (int i = 0; i < spawnPoints.Length / Random.Range(1, 3); i++)
        {
            AIMovable spawned = Instantiate<AIMovable>(enemies[Random.Range(0, enemies.Length)], spawnPoints[i].transform.position, Quaternion.identity);
            Vector3 spawnedPos = spawned.transform.position;

            VisualEffect vfx = Instantiate<VisualEffect>(spawnFX, new Vector3(spawnedPos.x, spawnedPos.y + 5f, spawnedPos.z), spawnFX.transform.rotation);
            Destroy(vfx.gameObject, 2);
            counter++;
        }

        SetEnemyAmount(counter);
        System.Array.Clear(spawnPoints, 0, spawnPoints.Length);
    }

    public void SpawnWithChance()
    {
        Transform[] spawnPoints = new Transform[0];
        int counter = 0;

        foreach (Transform child in dungeonUnit.transform)
        {
            if (child.name == "SpawnPoints")
            {
                spawnPoints = child.GetComponentsInChildren<Transform>();
                break;
            }
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawnpoints in dungeon");
            return;
        }

        for (int i = 0; i < spawnPoints.Length / Random.Range(1, 3); i++)
        {
            AIMovable spawned = Instantiate<AIMovable>(enemies[Random.Range(0, enemies.Length)], spawnPoints[i].transform.position, Quaternion.identity);
            Vector3 spawnedPos = spawned.transform.position;

            VisualEffect vfx = Instantiate<VisualEffect>(spawnFX, new Vector3(spawnedPos.x, spawnedPos.y + 5f, spawnedPos.z), spawnFX.transform.rotation);
            Destroy(vfx.gameObject, 2);
            counter++;
        }

        SetEnemyAmount(counter);
        System.Array.Clear(spawnPoints, 0, spawnPoints.Length);
    }

    [ContextMenu("Test Load")]
    public void TestLoad()
    {
        Vector3 _unitPos = dungeonUnit.transform.position;
        SpawnWithChance(Instantiate(unitsToLoad[5], new Vector3(_unitPos.x + offset.x, _unitPos.y, _unitPos.z), Quaternion.identity));
    }

    [ContextMenu("LoadUnit")]
    public void LoadUnit()
    {
        Vector3 _unitPos = dungeonUnit.transform.position;
        GameObject _unit = new GameObject();
        unitCounter++;

        #region  UnitPreparation

        if (dungeonUnit.name.Contains("Hall") == true)
        {
            xExtraOffset = -11.5f;
        }
        else
        {
            xExtraOffset = 0;
        }

        offset.x += xExtraOffset;

        if (unitCounter == unitsTillBoss)
        {
            _unit = Instantiate(unitsToLoad[unitsToLoad.Length - 1], new Vector3(_unitPos.x + offset.x, _unitPos.y, _unitPos.z), Quaternion.identity);
            SetCurrentDungeonUnit(_unit);
            SpawnWithChance(_unit);
            offset.x -= xExtraOffset;
            return;
        }

        int _unitId = Random.Range(0, unitsToLoad.Length - 1);
        if (dungeonUnit.name.Contains("Elevator") == true)
        {
            _unit = Instantiate(unitsToLoad[_unitId], new Vector3(_unitPos.x + offset.x, _unitPos.y + offset.y, _unitPos.z), Quaternion.identity);
        }
        else if (dungeonUnit.name.Contains("Square") == false)
        {
            _unit = Instantiate(unitsToLoad[_unitId], new Vector3(_unitPos.x + offset.x, _unitPos.y, _unitPos.z), Quaternion.identity);
        }
        else
        {
            _unit = Instantiate(unitsToLoad[0], new Vector3(_unitPos.x + offset.x, _unitPos.y, _unitPos.z), Quaternion.identity);
        }
        SpawnWithChance(_unit);
        #endregion

        offset.x -= xExtraOffset;

        SetCurrentDungeonUnit(_unit);
        DefineGamemode();
    }
}