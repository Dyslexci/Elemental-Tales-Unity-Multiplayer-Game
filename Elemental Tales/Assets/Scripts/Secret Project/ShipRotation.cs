﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 1.2.2
 *    @version 1.0.0
 *    
 *    Controls the direction of the arrow which shows the player which direction they will slingshot in.
 */

public class ShipRotation : MonoBehaviour
{
    public float angle;
    public float angularVelocity;

    /// <summary>
    /// Rotates the arrow to point at the mouse.
    /// </summary>
    void FixedUpdate()
    {
        Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle + -90, Vector3.forward);
        var step = angularVelocity * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle + -90, Vector3.forward), step);
    }
}
