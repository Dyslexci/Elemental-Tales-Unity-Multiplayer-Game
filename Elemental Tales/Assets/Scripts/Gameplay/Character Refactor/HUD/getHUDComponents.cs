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

    public GameObject getCollectible1Image()
    {
        return collectible1Image;
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
