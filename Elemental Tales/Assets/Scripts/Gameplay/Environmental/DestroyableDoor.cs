using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 110.0
 *    
 *    Allows assigned doors to be destroyed.
 */

public class DestroyableDoor : MonoBehaviourPun
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    public AudioSource destructionSound;

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
        photonView.RPC("damageDoorRPC", RpcTarget.AllBuffered, damage);
    }

    /// <summary>
    /// Destroys the door.
    /// </summary>
    private void destroyDoor()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// RPC triggering damage for all players on network
    /// </summary>
    /// <param name="damage"></param>
    [PunRPC] private void damageDoorRPC(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            StartCoroutine(DestroyDoor());
        }
    }

    IEnumerator DestroyDoor()
    {
        destructionSound.Play(0);
        while (destructionSound.isPlaying)
        {
            yield return new WaitForFixedUpdate();
        }
        destroyDoor();
    }
}
