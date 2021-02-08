using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 1.0.0
 *    
 *    Checks for player performing the stomping action onto the door, and removes the door when this is the case.
 */

public class SlamDoor : MonoBehaviour
{
    /// <summary>
    /// When the player enters the collider, checks to see if they are stomping - if so, delete the object.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if(collision.GetComponent<CharacterControllerPlayer1>().isStomping())
        {
            gameObject.SetActive(false);
        }
    }
}
