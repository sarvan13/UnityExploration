using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public float jumpStrength;
    public float moveStrength;

    private CapsuleCollider2D capsuleCollider;
    private static int distToGround;
    public int jumps;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = "Dino Man";
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) == true || Input.GetKeyDown(KeyCode.Space) == true) 
        {
            PlayerJump();
        }

        if (Input.GetKey(KeyCode.D) == true) 
        {
            myRigidBody.velocity = new Vector2(moveStrength, myRigidBody.velocity.y);
        }
        
        if (Input.GetKey(KeyCode.A) == true) 
        {
            myRigidBody.velocity = new Vector2(-1*moveStrength, myRigidBody.velocity.y);
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
}
