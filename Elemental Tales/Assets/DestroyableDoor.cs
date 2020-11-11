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

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void damageDoor(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            destroyDoor();
        }
    }
    private void destroyDoor()
    {
        gameObject.SetActive(false);
    }
}
