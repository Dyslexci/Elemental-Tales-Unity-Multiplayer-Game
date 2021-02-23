using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderPortalVertical : MonoBehaviour
{
    public BorderPortalVertical linkedPortal;
    public bool inTransit;
    float linkedPortalOffset;

    void Start()
    {
        if (linkedPortal.transform.position.y < 0)
        {
            linkedPortalOffset = .4f;
        }
        else
        {
            linkedPortalOffset = -.4f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && !inTransit)
        {
            collision.transform.position = new Vector2(collision.transform.position.x, linkedPortal.transform.position.y + linkedPortalOffset);
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
