using TMPro;
using UnityEngine;

public class UICoingHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinAmount;
    private ICanTrade trader;
    private void Start()
    {
        trader = GameObject.FindGameObjectWithTag("Player").GetComponent<ICanTrade>();
        trader.onTrade += OnTrade;
    }
    
    private void OnTrade(int gold) => coinAmount.text = gold.ToString();
}
