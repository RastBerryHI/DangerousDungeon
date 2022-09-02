using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GoldBehavior : MonoBehaviour
{
    [SerializeField] private GameObject gfx;
    private new AudioSource audio;
    private int goldAmount;
    private bool bIsPickedUp;

    public int GoldAmount
    {
        set
        {
            if (value < 0)
            {
                goldAmount = 0;
            }
            else
            {
                goldAmount = value;
            }
        }
    }

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    public void AddGold(ICanTrade trader, int goldAmount)
    {
        StartCoroutine(DelayAddGold(trader, goldAmount));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ICanTrade trader = other.GetComponent<ICanTrade>();
            AddGold(trader, goldAmount);
            audio.Play();
        }
    }

    private IEnumerator<WaitForSeconds> DelayAddGold(ICanTrade trader, int goldAmount)
    {
        Destroy(gfx);
        Destroy(GetComponent<BoxCollider>());

        trader.OnTrade(++trader.Gold);
        for (int i = 1; i < goldAmount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            trader.OnTrade(++trader.Gold);
        }

        Destroy(gameObject, audio.clip.length);
    }
}
