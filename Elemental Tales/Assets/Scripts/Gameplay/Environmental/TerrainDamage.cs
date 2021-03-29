using Photon.Pun;
using UnityEngine;

/**
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 1.2.1
 *
 *    Checks for the players presence, and applies (optionally elemental) damage to the player when they are present.
 */

public class TerrainDamage : MonoBehaviourPun
{
    [SerializeField] private string damageType;
    [SerializeField] private int damage;
    private StatController playerController;
    private float hitLast = 0;
    private float hitDelay = .75f;

    /// <summary>
    /// When the player enters the collider, check to see if it is the local player, and applies (optionally elemental) damage to that player object over time.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Time.time - hitLast < hitDelay | (collision.gameObject.GetPhotonView().IsMine == false && PhotonNetwork.IsConnected == true))
            {
                return;
            }
            playerController = collision.gameObject.GetComponent<StatController>();
            print("Damaged player for " + damage);
            playerController.DamageHealth(damage);
            hitLast = Time.time;
        }
    }
}