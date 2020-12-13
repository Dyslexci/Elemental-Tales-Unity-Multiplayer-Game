using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDamage : MonoBehaviour
{
    [SerializeField] private CharacterControllerPlayer1 player1;
    [SerializeField] private CharacterControllerPlayer1 player2;
    [SerializeField] private int damage;
    private float hitLast = 0;
    private float hitDelay = 2;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !collision.GetComponent<CharacterControllerPlayer1>().getElement().Equals("Water"))
        {
            if (Time.time - hitLast < hitDelay)
            {
                return;
            }

            if (collision.name.Equals("Player"))
            {
                player1.DamageHealth(-damage);
            }
            print("Taken" + damage + "damage");
            hitLast = Time.time;
        }
    }
}
