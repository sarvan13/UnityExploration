using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth;
    private Animator anim;
    private bool dead;
    [SerializeField] private float iFrameTime;
    [SerializeField] private int numFlashes;
    private SpriteRenderer spriteRenderer; 

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            anim.SetTrigger("hurt");
            //StartCoroutine(Invulnerability());
        }
        else
        {
            // game over anim
            anim.SetTrigger("die");
        }
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(9, 11, true);

        yield return new WaitForSeconds(1);

        Physics2D.IgnoreLayerCollision(9, 11, false);
    }
}
