using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterInteract : MonoBehaviour
{
    private bool inContact = false;
    private Collider2D collisionCollider;
    private Joint2D myJoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (collisionCollider == null)
        {
            GetComponent<RelativeJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
            return;
        }

        if(collisionCollider.name == "Player")
        {
            if(inContact && Input.GetButton("PushPull1"))
            {
                GetComponent<RelativeJoint2D>().connectedBody = collisionCollider.GetComponent<Rigidbody2D>();
            } else
            {
                GetComponent<RelativeJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
            }
        } else if(collisionCollider.name.Equals("Player 2"))
        {
            if (inContact && Input.GetButton("PushPull12"))
            {
                GetComponent<RelativeJoint2D>().connectedBody = collisionCollider.GetComponent<Rigidbody2D>();
            } else
            {
                GetComponent<RelativeJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            inContact = true;
            collisionCollider = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inContact = false;
        collisionCollider = null;
    }
}
