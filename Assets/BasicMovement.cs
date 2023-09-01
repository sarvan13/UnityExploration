using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public float jumpStrength;
    public float moveStrength;

    private CapsuleCollider2D capsuleCollider;
    private Animator anim;
    private static int distToGround;
    private int jumps;
    public int lives;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = "Dino Man";
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        lives = 3;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("grounded", checkGrounded());

        if (Input.GetKeyDown(KeyCode.W) == true || Input.GetKeyDown(KeyCode.Space) == true) 
        {
            PlayerJump();
        }

        if (Input.GetKey(KeyCode.D) == true) 
        {
            transform.localScale = new Vector3(1, 1, 1);
          
            myRigidBody.velocity = new Vector2(moveStrength, myRigidBody.velocity.y);

            if (anim.GetBool("grounded"))
            {
                anim.SetBool("run", true);
            }
        }
        
        else if (Input.GetKey(KeyCode.A) == true) 
        {
            transform.localScale = new Vector3(-1, 1, 1);
        
            myRigidBody.velocity = new Vector2(-1*moveStrength, myRigidBody.velocity.y);

            if (anim.GetBool("grounded"))
            {
                anim.SetBool("run", true);
            }
        }

        else 
        {
            anim.SetBool("run", false);
        }
    }

    void PlayerJump()
    {
        if (checkGrounded())
        {
            jumps = 0;
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpStrength);
            jumps += 1;
        }
        else if (jumps < 2)
        {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpStrength);
            jumps += 1;
        }

    }

    bool checkGrounded()
    {
        Vector2 position = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, 0.01f);

        if (hit.collider != null && hit.collider.gameObject.name == "Ground")
        {
            return true;
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Cactus")
        {
            myRigidBody.velocity = new Vector2(-3*myRigidBody.velocity.x, 0);
            lives -= 1;
        }
    }
}
