using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class collide : MonoBehaviourPun
{
    private bool playerPresent;
    [SerializeField] private Transform pos;

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos.position, 1.5f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player")
            {
                if (playerPresent == true)
                    return;
                Debug.Log("Player has entered the region");
                playerPresent = true;
            } else
            {
                if (playerPresent == false)
                    return;
                Debug.Log("Player has left the region");
                playerPresent = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(pos.position, 1.5f);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //        Debug.Log("Player entered lever");
    //        playerPresent = true;
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Debug.Log("Player left lever");
    //        playerPresent = true;
    //    }
    //}
}
