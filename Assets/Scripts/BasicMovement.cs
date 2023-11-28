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

    [SerializeField] private Health playerHealth;

    private CapsuleCollider2D capsuleCollider;
    private Animator anim;
    private static int distToGround;
    private int jumps;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = "Player";
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("grounded", checkGrounded());
        anim.SetBool("onWall", checkWalled());

        if (anim.GetBool("onWall")) jumps = 0;

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
        if (anim.GetBool("grounded"))
        {
            jumps = 0;
        }
        if (jumps < 2)
        {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpStrength);
            jumps += 1;
        }

    }

    bool checkGrounded()
    {
        Vector2 position = transform.position;
        float rayLength = 0.2f;
        int layerMask = ~(1 << 9); //Exclude layer 9 (Player Layer)
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, rayLength, layerMask);
        Debug.DrawLine(position, new Vector2(position.x, position.y - rayLength), Color.magenta, 2.5f);

        if (hit.collider != null && hit.collider.gameObject.name == "Ground")
        {
            return true;
        }
        return false;
    }

    bool checkWalled()
    {
        Vector2 position = transform.position;
        float rayLength = 0.2f;
        int layerMask = ~(1 << 9); //Exclude layer 9 (Player Layer)
        RaycastHit2D hit = Physics2D.Raycast(position, new Vector2(transform.localScale.x, 0), rayLength, layerMask);

        if (hit.collider != null && hit.collider.gameObject.name == "Wall")
        {
            return true;
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Cactus")
        {
            playerHealth.takeDamage(1);
        }
    }
}
