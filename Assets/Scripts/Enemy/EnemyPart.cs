using UnityEngine;

public class EnemyPart : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, Random.Range(5, 11));
    }
    
    private void OnEnable()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if(audio != null)
        {
            audio.Play();
        }
    }
}
