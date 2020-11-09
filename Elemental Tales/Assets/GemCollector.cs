using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollector : MonoBehaviour
{
    private double hitLast = 0;
    private double hitDelay = 0.2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(Time.time - hitLast < hitDelay)
        {
                return;
            }

            gameObject.SetActive(false);
            print("Collected");
            FindObjectOfType<GameMaster>().addCollectible1();

            hitLast = Time.time;
        }
    }
}
