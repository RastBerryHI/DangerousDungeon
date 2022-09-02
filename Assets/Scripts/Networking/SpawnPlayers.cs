using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject[] players;
    public Transform[] spawnPositions;

    private int index;

    private void Awake()
    {
        //index = PhotonNetwork.CountOfPlayers-1;
        //Vector3 randomPosition = spawnPositions[Random.Range(0, spawnPositions.Length)].position;
        //PhotonNetwork.Instantiate(players[index].name, randomPosition, Quaternion.identity);
    }
}
