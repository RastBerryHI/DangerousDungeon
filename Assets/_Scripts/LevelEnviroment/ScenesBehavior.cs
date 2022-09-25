using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ScenesBehavior : MonoBehaviour
{
    public static ScenesBehavior s_instance;
    [SerializeField] private float restartDelay;

    private void Awake()
    {
        if(s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(s_instance.gameObject);
        }
    }

    public void RestartGame()
    {
        StartCoroutine(DealyRestart());
    }

    private IEnumerator<WaitForSeconds> DealyRestart()
    {
        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(0);
    }
}
