using UnityEngine;
using UnityEngine.Events;
public interface ICanTrade
{
    public int Gold { get; set; }
    public bool CanTrade(int price);
    public event UnityAction<int> onTrade;
    public void OnTrade(int gold);
}
