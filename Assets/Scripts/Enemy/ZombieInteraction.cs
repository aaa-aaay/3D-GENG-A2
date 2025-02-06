using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieInteraction : MonoBehaviour, Damagable
{
    [SerializeField]private int health = 100;

    public void DesotryObject()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
       health -= amount;
        if (health <= 0)
        {
            DesotryObject();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
