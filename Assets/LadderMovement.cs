using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    public BasicMovement playerMovement;
    public Rigidbody2D myRigidBody;
    private Collider2D ladderCollider;
    public bool onLadder;
    [SerializeField] private int ladderSpeed;
    private float playerGravity;

    // Update is called once per frame
    void Update()
    {
        if (onLadder && Input.GetAxis("Vertical") != 0f)
        {
            playerMovement.setCanMove(false);
            myRigidBody.transform.position = new Vector2(ladderCollider.bounds.center.x, myRigidBody.transform.position.y);
            myRigidBody.gravityScale = 0f;

            if (Input.GetAxis("Vertical") > 0.1f)
            {
                myRigidBody.velocity = new Vector2(0, ladderSpeed);
            }
            else if (Input.GetAxis("Vertical") < -0.1f)
            {
                myRigidBody.velocity = new Vector2(0, -ladderSpeed);
            }
            else
            {
                myRigidBody.velocity = Vector2.zero;
            }
        }
        if (playerMovement.checkGrounded())
        {
            playerMovement.setCanMove(true);
            playerMovement.anim.SetBool("grounded", true);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Ladder" )
        {
            onLadder = true;
            ladderCollider = collider;
            playerGravity = myRigidBody.gravityScale;
        }
        else
        {
            Debug.Log("WTF");
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Ladder")
        {
            onLadder = false;
            playerMovement.setCanMove(true);
            myRigidBody.gravityScale = playerGravity;
        }
    }
}
