using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 1.0.0
 *    
 *    Controls the visual collectible counter on the HUD.
 */

public class Collectible1Counter : MonoBehaviour
{
    [SerializeField] private GameObject collectible1Holder;
    private int noCollectibles1;
    [SerializeField] private TMP_Text collectible1Number;

    /// <summary>
    /// Initialises the HUD objects this object needs to interact with.
    /// </summary>
    private void Start()
    {
        Debug.Log(GameObject.Find("PlayerHUDObject").name);
        collectible1Holder = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getCollectible1Holder();
        collectible1Number = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getCollectible1Text();
    }

    /// <summary>
    /// Updates the HUD objects with current data.
    /// </summary>
    void Update()
    {
        noCollectibles1 = FindObjectOfType<GameMaster>().getCollectible1();
        if (noCollectibles1 == 0)
        {
            collectible1Holder.SetActive(false);;
        } else
        {
            collectible1Holder.SetActive(true);
            collectible1Number.text = noCollectibles1.ToString();
        }
    }
}
