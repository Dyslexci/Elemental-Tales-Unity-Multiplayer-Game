using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HealthRefiller : MonoBehaviour
{
    bool beenPopped;
    public SpriteRenderer capImage;
    public GameObject healthExplodePrefab;

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
            collision.GetComponent<StatController>().DamageHealth(-2);
            GameObject.Find("Game Manager").GetComponent<GameMaster>().healthRefillCollect.Play(0);
            collision.GetComponent<PlayerInputs>().OnJumpInputDown();
            StartCoroutine(WaitToEndJump(collision.GetComponent<PlayerInputs>()));
            //collision.GetComponent<PlayerInputs>().OnJumpInputUp();
            capImage.color = new Color(0.2830189f, 0.2830189f, 0.2830189f, 1);
            transform.localScale = new Vector2(.3f, .3f);
            StartCoroutine(WaitToRespawn());
        }
    }

    IEnumerator WaitToEndJump(PlayerInputs player)
    {
        yield return new WaitForSeconds(.4f);
        player.OnJumpInputUp();
    }

    IEnumerator WaitToRespawn()
    {
        yield return new WaitForSeconds(300);
        beenPopped = false;
        capImage.color = Color.white;
        transform.localScale = new Vector2(.5f, .5f);
    }
}
