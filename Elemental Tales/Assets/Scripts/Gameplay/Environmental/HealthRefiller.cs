using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 2.0.0
 *    @version 1.0.1
 *    
 *    Allows the player to refill health when jumping on this object.
 */
public class HealthRefiller : MonoBehaviour
{
    bool beenPopped;
    public SpriteRenderer capImage;
    public GameObject healthExplodePrefab;

    /// <summary>
    /// When the player enters the collision boundary, triggers the health refill, sound, little jump, effects, and etc.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (beenPopped)
            return;

        if(collision.tag == "Player")
        {
            if (collision.gameObject.GetPhotonView().IsMine == false && PhotonNetwork.IsConnected == true)
                return;

            Instantiate(healthExplodePrefab, transform.position, transform.rotation);
            beenPopped = true;
            GameObject.Find("Game Manager").GetComponent<GameMaster>().DisplayOneTimeHintPopup("HealthRefill");
            collision.GetComponent<StatController>().DamageHealth(-2);
            GameObject.Find("Game Manager").GetComponent<GameMaster>().healthRefillCollect.Play(0);
            collision.GetComponent<PlayerInputs>().OnJumpInputDown();
            StartCoroutine(WaitToEndJump(collision.GetComponent<PlayerInputs>()));
            capImage.color = new Color(0.2830189f, 0.2830189f, 0.2830189f, 1);
            transform.localScale = new Vector2(.3f, .3f);
            StartCoroutine(WaitToRespawn());
        }
    }

    /// <summary>
    /// Waits .4 seconds to release the down key for the player, to reduce the size of the jump.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    IEnumerator WaitToEndJump(PlayerInputs player)
    {
        yield return new WaitForSeconds(.4f);
        player.OnJumpInputUp();
    }

    /// <summary>
    /// Respawns this object after 5 minutes realtime.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitToRespawn()
    {
        yield return new WaitForSeconds(300);
        beenPopped = false;
        capImage.color = Color.white;
        transform.localScale = new Vector2(.5f, .5f);
    }
}
