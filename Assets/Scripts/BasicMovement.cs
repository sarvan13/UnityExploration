using System.Collections;
using System.Collections.Generic;
using Cainos.LucidEditor;
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
    private int jumps;
    private Vector2 idleCapsuleSize;
    [SerializeField] private float crouchedScale;
    [SerializeField] private float crouchedCapsuleOffset;
    private float wallJumpCoolDown;
    [SerializeField] private float wallCoolDownTime = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = "Player";
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        idleCapsuleSize = capsuleCollider.size;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("grounded", checkGrounded());
        anim.SetBool("onWall", checkWalled());
        wallJumpCoolDown -= Time.deltaTime;

        if (anim.GetBool("onWall")) wallGravity();
        else myRigidBody.gravityScale = 1f;

        if (Input.GetKeyDown(KeyCode.W) == true || Input.GetKeyDown(KeyCode.Space) == true) 
        {
            PlayerJump();
        }
        PlayerCrouch();
        PlayerRun();
    }

    private void PlayerJump()
    {
        float horizontalJump = myRigidBody.velocity.x;

        if (anim.GetBool("grounded"))
        {
            jumps = 0;
        }
        else if (anim.GetBool("onWall") && wallJumpCoolDown <= 0f)
        {
            jumps = 1;
            horizontalJump -= transform.localScale.x;
            wallJumpCoolDown = wallCoolDownTime;
        }
        if (jumps < 2)
        {
            myRigidBody.velocity = new Vector2(horizontalJump, jumpStrength);
            jumps += 1;
        }
    }

    private void PlayerRun()
    {
        if (Input.GetKey(KeyCode.D) == true && !anim.GetBool("onWall")) 
        {
            transform.localScale = new Vector3(1, 1, 1);
          
            myRigidBody.velocity = new Vector2(moveStrength, myRigidBody.velocity.y);

            if (anim.GetBool("grounded"))
            {
                anim.SetBool("run", true);
            }
        }
        else if (Input.GetKey(KeyCode.A) == true && !anim.GetBool("onWall")) 
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

    private void PlayerCrouch()
    {
        if (Input.GetKeyDown(KeyCode.S) == true)
        {
            anim.SetBool("crouch", true);
            capsuleCollider.size = new Vector2(idleCapsuleSize.x * crouchedScale, idleCapsuleSize.x * crouchedScale);
            capsuleCollider.offset = new Vector2(capsuleCollider.offset.x, capsuleCollider.offset.y + crouchedCapsuleOffset);
        }
        else if (Input.GetKeyUp(KeyCode.S) == true)
        {
            anim.SetBool("crouch", false);
            capsuleCollider.size = new Vector2(idleCapsuleSize.x, idleCapsuleSize.y);
            capsuleCollider.offset = new Vector2(capsuleCollider.offset.x, capsuleCollider.offset.y - crouchedCapsuleOffset);
        }
    }

    bool checkGrounded()
    {
        Vector2 position = transform.position;
        float rayLength = 0.2f;
        int layerMask = ~(1 << 9); //Exclude layer 9 (Player Layer)
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, rayLength, layerMask);

        if (hit.collider != null && (hit.collider.gameObject.layer == 8 || hit.collider.gameObject.layer == 9))
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
        Debug.DrawLine(position, new Vector2(position.x + transform.localScale.x * rayLength, position.y), Color.magenta, 0.5f);

        if (hit.collider != null && hit.collider.gameObject.layer == 10)
        {
            return true;
        }
        else if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.layer);
        }
        return false;
    }

    private void wallGravity()
    {
        myRigidBody.gravityScale = 0.2f;
        jumps = 1;

        if (transform.localScale.x < 0)
        {
            // Wall is on the left
            if (Input.GetKey(KeyCode.A)) 
            {
                // Stay in place if pressing into the wall
                myRigidBody.velocity = Vector2.zero;
                myRigidBody.gravityScale = 0f;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                // Fall at normal speed if pressing off the wall
                myRigidBody.gravityScale = 1f;
                myRigidBody.velocity = new Vector2(-1f * transform.localScale.x, 0f);
            }
        }
        else
        {
            // Wall is on the right
            if (Input.GetKey(KeyCode.D))
            {
                // Stay in place if pressing into the wall
                myRigidBody.velocity = Vector2.zero;
                myRigidBody.gravityScale = 0f;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                // Fall at normal speed if pressing off the wall
                myRigidBody.gravityScale = 1f;
                myRigidBody.velocity = new Vector2(-1f*transform.localScale.x, 0f);
            }
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
        {
            myRigidBody.gravityScale = 1f;
        }
    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Cactus")
        {
            playerHealth.takeDamage(1);
        }
    }
}
