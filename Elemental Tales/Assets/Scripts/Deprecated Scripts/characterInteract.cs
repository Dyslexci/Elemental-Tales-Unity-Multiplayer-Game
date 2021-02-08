using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 0.3.0
 *    @version 1.0.0
 *    
 *    Used to provide ability for player object to connect to a rigidbody and push/pull it around. Didn't work correctly, now deprecated.
 */
public class characterInteract : MonoBehaviour
{
    private bool inContact = false;
    private Collider2D collisionCollider;
    private Joint2D myJoint;

    void Update()
    {
        if (collisionCollider == null)
        {
            GetComponent<RelativeJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
            return;
        }

            if(inContact && Input.GetButton("PushPull1"))
            {
                GetComponent<RelativeJoint2D>().connectedBody = collisionCollider.GetComponent<Rigidbody2D>();
            } else
            {
                GetComponent<RelativeJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
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
