using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    const int MaxHealth = 3;
    public int CurrentHealth;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected int TakeDamage(int Damage)
    {
        CurrentHealth -= Damage;
        return CurrentHealth;
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {
        if (Collision.collider.CompareTag("Boomerang"))
        {
            BoomerangFunctionality BoomerangScript = Collision.gameObject.GetComponent<BoomerangFunctionality>();
            TakeDamage(BoomerangScript.Damage);
        }
    }

}
