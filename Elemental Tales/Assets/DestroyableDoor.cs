using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableDoor : MonoBehaviour
{
    private int maxHealth = 100;
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
