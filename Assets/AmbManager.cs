using UnityEngine;

public class AmbManager : MonoBehaviour
{
    public static AmbManager s_instance;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(s_instance.gameObject);
        }
    }

    public void StopAmb()
    {
        s_instance.GetComponent<AudioSource>().Stop();
    }
}
