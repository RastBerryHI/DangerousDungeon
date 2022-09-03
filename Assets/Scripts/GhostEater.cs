using UnityEngine;

public class GhostEater : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    public Vector3 direction;
    public Character master;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        if(this != null) transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Character")
        {
            Debug.Log(master.MaxHealth);
            other.GetComponent<Character>().EarnDamage(master.Damage + master.CurrentWeapon.Damage, gameObject);
            Destroy(gameObject);
            //master.Heal(master.MaxHealth);
        }
    }
}
