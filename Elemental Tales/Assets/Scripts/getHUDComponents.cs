using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class getHUDComponents : MonoBehaviour
{
    [SerializeField] private Image elementOrb;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Image[] manas;
    [SerializeField] private GameObject collectible1Image;
    [SerializeField] private TMP_Text collectible1Text;

    public Image getElementOrb()
    {
        Debug.Log("detched element orb");
        return elementOrb;
    }

    public Image[] getHearts()
    {
        Debug.Log("Hearts got");
        return hearts;
    }

    public Image[] getManas()
    {
        Debug.Log("Mana got");
        return manas;
    }

    public GameObject getCollectible1Image()
    {
        Debug.Log("Added collectible 1 image");
        return collectible1Image;
    }

    public TMP_Text getCollectible1Text()
    {
        Debug.Log("Added collectible 1 text");
        return collectible1Text;
    }
}
