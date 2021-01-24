using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    Controls the visual collectible counter on the HUD.
 */

public class Collectible1Counter : MonoBehaviour
{
    [SerializeField] private GameObject collectible1Image;
    private int noCollectibles1;
    [SerializeField] private TMP_Text collectible1Number;

    private void Start()
    {
        Debug.Log(GameObject.Find("PlayerHUDObject").name);
        collectible1Image = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getCollectible1Image();
        collectible1Number = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getCollectible1Text();
    }

    void Update()
    {
        noCollectibles1 = FindObjectOfType<GameMaster>().getCollectible1();
        if (noCollectibles1 == 0)
        {
            collectible1Image.SetActive(false);;
            collectible1Number.enabled = false;
        } else
        {
            collectible1Image.SetActive(true);
            collectible1Number.enabled = true;
            collectible1Number.text = noCollectibles1.ToString();
        }
    }
}
