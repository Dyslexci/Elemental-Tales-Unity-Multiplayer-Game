using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterInteract : MonoBehaviour
{
    private bool inContact = false;
    private Collider2D collisionCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (collisionCollider == null)
            return;

        if(collisionCollider.name.Equals("Player"))
        {
            if(inContact && Input.GetButton("PushPull1"))
            {
                
            }
        } else
        {

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inContact = true;
        collisionCollider = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inContact = false;
        collisionCollider = null;
    }
}
