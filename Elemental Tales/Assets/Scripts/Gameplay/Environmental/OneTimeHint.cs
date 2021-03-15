using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

/** 
 *    @author Matthew Ahearn
 *    @since 1.2.0
 *    @version 1.0.0
 *    
 *    Displays a hint a single time, for need-to-know information that isn't important enough to be prompted every time.
 */
public class OneTimeHint : MonoBehaviourPun
{
    public string hintText;
    bool hasBeenTriggered;

    [SerializeField] Transform pos;
    [SerializeField] float lengthX = 1.5f;
    [SerializeField] float lengthY = 1.5f;
    [SerializeField] private LayerMask layer;
    public float angle;

    TMP_Text hintTextObj;
    GameObject hintHolder;
    CanvasGroup panel;
    Image hintImage;

    private void Start()
    {
        hintTextObj = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintText();
        hintHolder = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().GetHintHolder();
        hintImage = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintContainer();
        panel = hintHolder.GetComponentInChildren<CanvasGroup>();
    }

    private void FixedUpdate()
    {
        if(!hasBeenTriggered)
            checkPresent();
    }


    /// <summary>
    /// Checks for the players presence based off Physics2D collider circles and displays the hint if the player has entered the switch collider.
    /// </summary>
    private void checkPresent()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, new Vector2(lengthX, lengthY), angle, layer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player")
            {
                if (colliders[i].gameObject.GetPhotonView().IsMine)
                {
                    if (!hasBeenTriggered)
                    {
                        Debug.Log("Checkpresent worked");
                        GameObject.Find("Game Manager").GetComponent<GameMaster>().hintSound.Play(0);
                        hasBeenTriggered = true;
                        StartCoroutine(WaitHideHint());
                    }
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Draws a cisual representation of the switch collider in the editor.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, new Vector3(lengthX, lengthY, 1));
    }

    /// <summary>
    /// Coroutine displaying the hint to the player.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitHideHint()
    {
        hintHolder.SetActive(true);
        StartCoroutine("JumpInHintHolder");
        hintTextObj.text = hintText;
        yield return new WaitForSeconds(2);
        StartCoroutine("FadeHintHolder");
    }

    /// <summary>
    /// Coroutine making the hint jump into place.
    /// </summary>
    /// <returns></returns>
    IEnumerator JumpInHintHolder()
    {
        hintImage.transform.localScale = new Vector3(5, 5, 5);

        while (hintImage.transform.localScale.x > 1)
        {
            yield return new WaitForFixedUpdate();
            hintImage.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
        }
        hintImage.transform.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// Coroutine making the hint fade out after it has been on screen enough time.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeHintHolder()
    {
        while (panel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            panel.alpha -= 0.05f;
        }

        hintHolder.SetActive(false);
        panel.alpha = 1;
        Destroy(gameObject);
    }
}
