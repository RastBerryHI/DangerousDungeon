using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    private Knight knight;
    private void Start()
    {
        //knight = FindObjectOfType<Knight>();
        //knight.onDie.AddListener(ReloadGame);
    }
    private void ReloadGame()
    {
        StartCoroutine(DelayReoad());
    }

    private IEnumerator<WaitForSeconds> DelayReoad()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
