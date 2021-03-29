using Photon.Pun;
using UnityEngine;

/**
 *    @author Matthew Ahearn
 *    @since 2.3.0
 *    @version 1.0.1
 *
 *    Adds permanent health to the player.
 */

public class HealthUpgrader : MonoBehaviourPun
{
    private double hitLast = 0;
    private double hitDelay = 0.2;

    /// <summary>
    /// On player object collision, adds 1 to the maximum player health and lets game manager take care of tracking stats.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetPhotonView().IsMine == false && PhotonNetwork.IsConnected == true)
                return;
            if (Time.time - hitLast < hitDelay)
                return;
            photonView.RPC("DeleteHealthUpgrade", RpcTarget.AllBuffered);
            GameObject.Find("Game Manager").GetComponent<GameMaster>().DisplayOneTimeHintPopup("HealthUpgrade");
            hitLast = Time.time;
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Sends an RPC to delete the health object and add a permanent health to both players.
    /// </summary>
    [PunRPC]
    private void DeleteHealthUpgrade()
    {
        Debug.Log("PUN: deleteGem() has been called via the RPC.");
        GameObject.Find("Game Manager").GetComponent<GameMaster>().healthCollector.Play(0);
        GameObject.Find("Game Manager").GetComponent<GameMaster>().getPlayer().GetComponent<StatController>().IncreaseMaxHealth();
        GameObject.Find("Game Manager").GetComponent<GameMaster>().AddHealthUpgrade();
        this.gameObject.SetActive(false);
    }
}