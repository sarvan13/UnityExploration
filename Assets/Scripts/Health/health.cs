using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damage)
    {
        currentHealth = Math.Clamp(currentHealth - 1, 0, startingHealth);

        if (currentHealth > 0)
        {
            // take damage anim
        }
        else
        {
            // game over anim
        }
    }
}
