using UnityEngine;

public class RanNet : MonoBehaviour
{
    public static RanNet s_instance;
    private System.Random rnd;
    private int seed;

    private void Awake()
    {
        
        if (s_instance != null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(s_instance.gameObject);
        }

        seed = Random.Range(0, 10000);


        rnd = new System.Random(seed);
    }

    public int Range(int min, int max)
    {
        return rnd.Next(min, max);
    }
}
