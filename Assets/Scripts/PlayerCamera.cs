using System;
using CharacterComponents;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Movable player;
    
    [SerializeField] private Vector3 offset;
    [SerializeField] private float step;

    private Transform trm;

    private void Awake()
    {
        trm = transform;
    }

    private void LateUpdate()
    {
        if (!player)
        {
            return;
        }
        
        var pos = player.transform.position;
        trm.position = Vector3.Lerp(trm.position, new Vector3(pos.x + offset.x, pos.y + offset.y, pos.z + offset.z), step);
    }
}
