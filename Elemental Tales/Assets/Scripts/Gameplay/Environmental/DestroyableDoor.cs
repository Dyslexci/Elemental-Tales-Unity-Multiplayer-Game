using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    Allows assigned doors to be destroyed.
 */

public class DestroyableDoor : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    /// <summary>
    /// Sets the current health of the door.
    /// </summary>
    private void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Damages the health of the door with the given damage parameter, and destroys the door if the health drops below 0.
    /// </summary>
    /// <param name="damage"></param>
    public void damageDoor(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            destroyDoor();
        }
    }

    /// <summary>
    /// Destroys the door.
    /// </summary>
    private void destroyDoor()
    {
        gameObject.SetActive(false);
    }
}
