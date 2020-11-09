using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Collectible1Counter : MonoBehaviour
{
    [SerializeField] private Image collectible1Image;
    private int noCollectibles1;
    [SerializeField] private TMP_Text collectible1Number;

    // Update is called once per frame
    void Update()
    {
        noCollectibles1 = FindObjectOfType<GameMaster>().getCollectible1();
        if (noCollectibles1 == 0)
        {
            collectible1Image.enabled = false;
            collectible1Number.enabled = false;
        } else
        {
            collectible1Image.enabled = true;
            collectible1Number.enabled = true;
            collectible1Number.text = noCollectibles1.ToString();
        }
    }
}
