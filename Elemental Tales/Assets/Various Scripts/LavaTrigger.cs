using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTrigger : MonoBehaviour
{
    [SerializeField] private CharacterControllerPlayer1 player1;
    [SerializeField] private Health player2;
    private float hitLast = 0;
    private float hitDelay = 2;

    /**private void OnTriggerEnter2D(Collider2D collision)
    {
        damagePlayer(collision.name);
    }**/

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !collision.GetComponent<CharacterControllerPlayer1>().getElement().Equals("Fire"))
        {
            if (Time.time - hitLast < hitDelay)
            {
                return;
            }

            if (collision.name.Equals("Player"))
            {
                print("Player 1 has entered lava");
                player1.DamageHealth(-2);
            }
            else
            {
                print("Player 2 has entered lava");
                //player2.takeDamage(-2);
            }
            print("Taken damage");
            hitLast = Time.time;
        }
    }

    /**private void damagePlayer(string name)
    {
        if(name.Equals("Player"))
        {
            player1.DamageHealth(2);
        } else
        {
            player2.DamageHealth(2);
        }
    }**/
}
