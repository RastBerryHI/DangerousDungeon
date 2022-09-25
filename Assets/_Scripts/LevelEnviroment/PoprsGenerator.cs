using UnityEngine.UI;
using UnityEngine;

public class PoprsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] wallDecor;
    [SerializeField] private GameObject[] pot;
    [SerializeField] private GameObject[] tables;
    [SerializeField] private GameObject[] boxes;
    [SerializeField] private GameObject[] woodPlanks;
    [SerializeField] private GameObject[] grounds;

    [SerializeField] private Seller seller;
    [SerializeField] private Transform sellerPos;
    [SerializeField] private float sellerChance;

    private void Start()
    {
        GenerateEnviroment();
    }

    private void GenerateEnviroment()
    {
        if (Random.value > sellerChance)
        {
            Instantiate<Seller>(seller, sellerPos.position, Quaternion.identity, transform.parent);
            foreach(GameObject table in tables)
            {
                table.SetActive(false);
            }
            foreach(GameObject box in boxes)
            {
                box.SetActive(false);
            }
            return;
        }

        int wallDecorChance = RanNet.s_instance.Range(0, 3);

        switch (wallDecorChance)
        {
            case 0:
                foreach(GameObject wd in wallDecor)
                {
                    wd.SetActive(true);
                }
                break;
            case 1:
                int subWdChance = RanNet.s_instance.Range(0, 2);
                switch (subWdChance)
                {
                    case 0:
                        for(int i = 0; i < wallDecor.Length/2; i++)
                        {
                            wallDecor[i].SetActive(true);
                        }
                        break;
                    case 1:
                        for (int i = wallDecor.Length / 2; i > 0; i--)
                        {
                            wallDecor[i].SetActive(true);
                        }
                        break;
                }
                break;
            case 2:
                foreach (GameObject wd in wallDecor)
                {
                    wd.SetActive(false);
                }
                break;
        }

        int potChance = RanNet.s_instance.Range(0, 2);
        switch (potChance)
        {
            case 0:
                foreach(GameObject p in pot)
                {
                    p.SetActive(true);
                }
                break;
            case 1:
                foreach (GameObject p in pot)
                {
                    p.SetActive(false);
                }
                break;
        }

        int tableChance = RanNet.s_instance.Range(0, 2);

        switch (tableChance)
        {
            case 0:
                foreach (GameObject t in tables)
                {
                    t.SetActive(true);
                }
                break;
            case 1:
                foreach (GameObject t in tables)
                {
                    t.SetActive(false);
                }
                break;
        }

        int boxChance = RanNet.s_instance.Range(0, 3);

        switch (boxChance)
        {
            case 0:
                foreach (GameObject box in boxes)
                {
                    box.SetActive(true);
                }
                break;
            case 1:
                int subLampChance = RanNet.s_instance.Range(0, 2);
                switch (subLampChance)
                {
                    case 0:
                        for (int i = 0; i < boxes.Length / 2; i++)
                        {
                            boxes[i].SetActive(true);
                        }
                        break;
                    case 1:
                        for (int i = boxes.Length / 2; i > 0; i--)
                        {
                            boxes[i].SetActive(true);
                        }
                        break;
                }
                break;
            case 2:
                foreach (GameObject box in boxes)
                {
                    box.SetActive(false);
                }
                break;
        }
    }
}
