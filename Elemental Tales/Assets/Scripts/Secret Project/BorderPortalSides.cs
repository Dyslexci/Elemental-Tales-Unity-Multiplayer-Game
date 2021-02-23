using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderPortalSides : MonoBehaviour
{
    public BorderPortalSides linkedPortal;
    public bool inTransit;
    float linkedPortalOffset;

    // Start is called before the first frame update
    void Start()
    {
        if(linkedPortal.transform.position.x < 0)
        {
            linkedPortalOffset = .4f;
        } else
        {
            linkedPortalOffset = -.4f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && !inTransit)
        {
            collision.transform.position = new Vector2(linkedPortal.transform.position.x + linkedPortalOffset, collision.transform.position.y);
            linkedPortal.inTransit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && inTransit)
        {
            inTransit = false;
        }
    }




    void OnValidate()
    {
        if (linkedPortal != null)
        {
            linkedPortal.linkedPortal = this;
        }
    }
}
