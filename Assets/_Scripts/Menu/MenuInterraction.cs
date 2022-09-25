using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MenuInterraction : MonoBehaviour
{
    [SerializeField] private MenuCamera menuCamera;
    [SerializeField] private GameObject playButton;
    [SerializeField] private LayerMask characterMask;

    [SerializeField] private GameObject confirmButton;
    [SerializeField] private GameObject door;
    [SerializeField] private Image cullis;

    [SerializeField] private Animator knight;
    [SerializeField] private Animator necromancer;
    

    private int characterSelected;
    private bool bIsSelected;

    public void RemoveMenu()
    {
        StartCoroutine(DelayTurnOffMenu());
    }

    public void ShowMenu()
    {
        playButton.SetActive(true);
    }

    private IEnumerator<WaitForSeconds> DelayTurnOffMenu()
    {
        yield return new WaitForSeconds(0.06f);
        playButton.SetActive(false);
    }

    public void SelectCharacter()
    {
        if(characterSelected == 1)
        {
            SelectKnight();
        }
        else if(characterSelected == 2)
        {
            SelectNecromancer();
        }

        confirmButton.SetActive(false);
        bIsSelected = true;
        cullis.DOColor(Color.black, 3);

        StartCoroutine(DelayLoad());
        PlayerPrefs.SetInt("Character", characterSelected);
    }

    private IEnumerator<WaitForSeconds> DelayLoad()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(1);
    }

    private void SelectKnight()
    {
        knight.SetFloat("Velocity", 1);
        Vector3 dir = (door.transform.position - knight.transform.position).normalized;
        knight.transform.LookAt(door.transform);

        knight.transform.DOMove(door.transform.position, 10f);
    }

    private void SelectNecromancer()
    {
        necromancer.SetFloat("Velocity", 1);
        Vector3 dir = (door.transform.position - necromancer.transform.position).normalized;
        necromancer.transform.LookAt(door.transform);

        necromancer.transform.DOMove(door.transform.position, 10f);
    }

    private void Update()
    {
        if (bIsSelected == true) return;

#if UNITY_ANDROID
        if(Input.GetTouch(0).phase == TouchPhase.Began)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if(Physics.Raycast(ray, out hit, characterMask) == true)
            {
                if (menuCamera.tweenStatus == true) return;

                if (characterSelected == 1 || characterSelected == 2)
                {
                    confirmButton.SetActive(true);
                }

                if (hit.transform.name == "Player_Necromancer")
                {
                    characterSelected = 2;
                    menuCamera.MoveToNecromancer();
                }
                else if(hit.transform.name == "Player_Knight")
                {
                    characterSelected = 1;
                    menuCamera.MoveToKnight();
                }
            }
        }
#else
        if (Input.GetMouseButton(0) == true)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, characterMask) == true)
            {
                if (menuCamera.tweenStatus == true) return;

                if(characterSelected == 1 || characterSelected == 2)
                {
                    confirmButton.SetActive(true);
                }

                if (hit.transform.name == "Player_Necromancer")
                {
                    characterSelected = 2;
                    menuCamera.MoveToNecromancer();
                }
                else if(hit.transform.name == "Player_Knight")
                {
                    characterSelected = 1;
                    menuCamera.MoveToKnight();
                }
            }
        }
#endif
    }
}
