using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : MonoBehaviour
{
    const int MaxHealth = 3;
    public int CurrentHealth;
    public Slider HealthSlider;
    public bool CanTakeDamage;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        HealthSlider = GetComponentInChildren<Slider>();
        HealthSlider.maxValue = MaxHealth;
        HealthSlider.minValue = 0;
        HealthSlider.value = CurrentHealth;

        CanTakeDamage = true;

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected int TakeDamage(int Damage)
    {
        CurrentHealth -= Damage;
        HealthSlider.value = CurrentHealth;
        if(CurrentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
        return CurrentHealth;
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {
        if (Collision.collider.CompareTag("Boomerang"))
        {
            BoomerangFunctionality BoomerangScript = Collision.gameObject.GetComponent<BoomerangFunctionality>();
            if ( BoomerangScript.CanApplyDamage && CanTakeDamage) 
            {
                StartCoroutine(DamageCooldown());
                StartCoroutine(BoomerangScript.DamageCooldown());
                TakeDamage(BoomerangScript.Damage);
            }

        }
    }

    private IEnumerator DamageCooldown()
    {
        CanTakeDamage = false;
        yield return new WaitForSeconds(0.25f);
        CanTakeDamage = true;
    }
}
