using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangFunctionality : MonoBehaviour
{

    private const int MaxBounces = 3;
    public int BouncesRemaining;
    public int Damage;

    void Start()
    {
        if(Damage == 0)
        {
            Damage = 1;
        }
        BouncesRemaining = MaxBounces;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
