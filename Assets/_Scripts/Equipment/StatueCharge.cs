using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueCharge : MonoBehaviour
{
    [SerializeField] private GameObject wonder;

    private void Start()
    {
        if(wonder == null)
        {
            WarnerMessages.UnassingedField("wonder");
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerPockets _pockets = other.GetComponent<PlayerPockets>();
            if(_pockets == null) 
            {
                WarnerMessages.NoComponent("PlayerPockets", other);
                return;
            }

            if(_pockets.WonderPocket == null)
            {
                WarnerMessages.NoComponent("Transform", _pockets);
                return;
            }

            if(_pockets.WonderPocket.childCount == 0)
            {
                GameObject _wonder = Instantiate<GameObject>(wonder, _pockets.transform.parent.position, Quaternion.identity);

                _wonder.transform.parent = _pockets.transform;
                Destroy(this);
            }
        }
    }
}
