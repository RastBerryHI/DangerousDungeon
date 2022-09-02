using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

enum ElevatorStates : sbyte
{
    Arrived,
    Move
}

public class ElevatorBehaviour : MonoBehaviour
{
    [SerializeField] private Transform breakPoint;
    [SerializeField] private Transform spawnPointsHolder;

    [SerializeField] private ElevatorStates elevatorState;
    [SerializeField] private float speed;

    private float initialDistance;
    private int count;
    private bool bIsLoopEnded;

    private void Start()
    {
        LevelGenerator.s_levelGenerator.onWaveCleared.AddListener(MoveToNextPoint);
        Random.Range(transform.position.y, breakPoint.position.y);

        initialDistance = Vector3.Distance(transform.position, breakPoint.position);

    }

    private void Update()
    {
        if (LevelGenerator.s_levelGenerator.Waves == 0)
        {
            elevatorState = ElevatorStates.Move;
        }

        if (elevatorState == ElevatorStates.Move)
        {
            if (Vector3.Distance(transform.position, breakPoint.position) > 0.5f)
            {
                transform.position = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x, breakPoint.transform.position.y, transform.position.z), speed * Time.deltaTime);
            }
        }
    }


    [ContextMenu("MoveToNextPoint")]
    private void MoveToNextPoint()
    {
        spawnPointsHolder.parent = transform;
        elevatorState = ElevatorStates.Move;
        StartCoroutine(DelayStop());
        // currentTween = transform.DOMoveY(breakPoints[currentBreakPoint].transform.position.y, 5);
        // currentTween.onComplete += UnparentSpawns;
    }

    private void UnparentSpawns() => spawnPointsHolder.parent = LevelGenerator.s_levelGenerator.DungeonUnit.transform;

    private IEnumerator<WaitForSeconds> DelayStop()
    {
        yield return new WaitForSeconds(Random.Range(4, 9));
        UnparentSpawns();
        elevatorState = ElevatorStates.Arrived;
        LevelGenerator.s_levelGenerator.SpawnWithChance();
    }
}
