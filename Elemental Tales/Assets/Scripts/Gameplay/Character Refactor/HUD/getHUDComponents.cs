using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    Returns the HUD components to any enquiring script, to allow the objects to each interface with the HUD.
 */

public class getHUDComponents : MonoBehaviour
{
    [SerializeField] private Image elementOrb;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Image[] manas;
    [SerializeField] private GameObject collectible1Holder;
    [SerializeField] private TMP_Text collectible1Text;
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private Image hintContainer;
    [SerializeField] private GameObject HintHolder;

    private void Start()
    {
        HintHolder.SetActive(false);
    }

    public Image getElementOrb()
    {
        return elementOrb;
    }

    public Image[] getHearts()
    {
        return hearts;
    }

    public Image[] getManas()
    {
        return manas;
    }

    public GameObject getCollectible1Holder()
    {
        return collectible1Holder;
    }

    public TMP_Text getCollectible1Text()
    {
        return collectible1Text;
    }

    public TMP_Text getHintText()
    {
        return hintText;
    }

    public Image getHintContainer()
    {
        return hintContainer;
    }

    public GameObject GetHintHolder()
    {
        return HintHolder;
    }
}
