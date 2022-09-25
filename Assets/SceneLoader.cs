using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        //knight = FindObjectOfType<Knight>();
        //knight.onDie.AddListener(ReloadGame);
    }
    private void ReloadGame()
    {
        DelayReload();
    }

    private async void DelayReload()
    {
        await UniTask.Delay(5000);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
