using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

/** 
 *    @author Matthew Ahearn
 *    @since 1.2.0
 *    @version 1.0.2
 *    
 *    Displays a hint a single time, for need-to-know information that isn't important enough to be prompted every time.
 */
public class OneTimeHint : CheckPresentController
{
    UIHintController hintController;

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
        hintController = GameObject.Find("Game Manager").GetComponent<UIHintController>();

        hintTextObj = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintText();
        hintHolder = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().GetHintHolder();
        hintImage = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintContainer();
        panel = hintHolder.GetComponentInChildren<CanvasGroup>();
    }

    private void FixedUpdate()
    {
        if(!hasBeenTriggered)
            if(CheckPresentBox(pos, lengthX, lengthY, angle, layer))
            {
                hasBeenTriggered = true;
                hintController.StartHintDisplay(hintText, 5);
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
}
