using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float currentHealth;

    void Start()
    {
        currentHealth = health;
    }

    public int ComputeHealth(){
        if(health < currentHealth)      //When the skeleton is hurt
        {
            currentHealth = health;
            return 0;
        }

        else if(health <= 0)        //When the skeleton dies
        {
            return 1;
        }
        return 2;
    }
}
