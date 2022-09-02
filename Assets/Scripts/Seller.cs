using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Seller : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private Artifact[] artifacts;
    [SerializeField] private Potion[] potions;

    [SerializeField] private int weaponPrice;
    [SerializeField] private int potionPrice;
    [SerializeField] protected int artifactPrice;

    [Header("Shop popup")]
    [SerializeField] private CanvasRenderer shopPanel;
    
    [Space]
    
    [SerializeField] private GameObject weaponRow;
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponCost;
    
    [Space]

    [SerializeField] private GameObject artifactRow;
    [SerializeField] private TextMeshProUGUI artifactName;
    [SerializeField] private Image artifactIcon;
    [SerializeField] private TextMeshProUGUI artifactCost;

    [Space]

    [SerializeField] private GameObject potionRow;
    [SerializeField] private TextMeshProUGUI potionName;
    [SerializeField] private Image potionIcon;
    [SerializeField] private TextMeshProUGUI potionCost;

    private Weapon weapon;
    private Artifact artifact;
    private Potion potion;

    private ICanTrade tradeWith;
    private Character characterToTrade;
    private Weapon chosen;
    private Artifact _artifact;
    private Potion potionObj;

    public int WeaponPrice => weaponPrice;
    public int PotionPrice => potionPrice;
    public int ArtifactRow => artifactPrice;

    private void Start()
    {
        chosen = weapons[Random.Range(0, weapons.Length)];
        _artifact = artifacts[Random.Range(0, artifacts.Length)];
        potionObj = potions[Random.Range(0, potions.Length)];

        weapon = Instantiate<Weapon>(chosen, Vector3.zero, chosen.transform.rotation, transform);
        potion = Instantiate<Potion>(potionObj, Vector3.zero, Quaternion.identity, transform);
        artifact = Instantiate<Artifact>(_artifact, Vector3.zero, _artifact.transform.rotation, transform);

        weapon.gameObject.SetActive(false);
        potion.gameObject.SetActive(false);
        artifact.gameObject.SetActive(false);


        weaponName.text = chosen.sprite.name;
        weaponIcon.sprite = chosen.sprite;
        weaponCost.text = weaponPrice.ToString();

        potionName.text = potion.sprite.name;
        potionIcon.sprite = potion.sprite;
        potionCost.text = potionPrice.ToString();

        artifactName.text = artifact.sprite.name;
        artifactIcon.sprite = artifact.sprite;
        artifactCost.text = artifactPrice.ToString();

        shopPanel.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            tradeWith = other.GetComponent<ICanTrade>();
            characterToTrade = other.GetComponent<Character>();

            if (tradeWith != null && characterToTrade != null)
            {
                ShowShop(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ShowShop(false);

            tradeWith = null;
            characterToTrade = null;
        }
    }

    private void ShowShop(bool status) => shopPanel.gameObject.SetActive(status);

    public void OnWeaponBuy()
    {
        Debug.Log(tradeWith.Gold);
        if (tradeWith.CanTrade(weaponPrice))
        {
            weapon.gameObject.SetActive(true);
            weapon.transform.position = characterToTrade.transform.position;

            Destroy(weaponRow.gameObject);
        }
    }

    public void OnArtifactBuy()
    {
        if (tradeWith.CanTrade(artifactPrice))
        {
            artifact.gameObject.SetActive(true);
            artifact.transform.position = characterToTrade.transform.position;

            Destroy(artifactRow.gameObject);
        }
    }

    public void OnPotionBuy()
    {
        if (tradeWith.CanTrade(potionPrice))
        {
            potion.gameObject.SetActive(true);
            potion.transform.position = characterToTrade.transform.position;

            Destroy(potionRow.gameObject);
        }
    }

    private void OnDestroy()
    {
        System.Array.Clear(weapons, 0, weapons.Length);
        System.Array.Clear(artifacts, 0, artifacts.Length);
        System.Array.Clear(potions, 0, potions.Length);
    }
}
