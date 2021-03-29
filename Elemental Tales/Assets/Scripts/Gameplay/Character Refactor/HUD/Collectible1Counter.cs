using TMPro;
using UnityEngine;

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
    private SafeFloat noCollectibles1;
    [SerializeField] private TMP_Text collectible1Number;

    /// <summary>
    /// Initialises the HUD objects this object needs to interact with.
    /// </summary>
    private void Start()
    {
        noCollectibles1 = new SafeFloat(0);
        Debug.Log(GameObject.Find("PlayerHUDObject").name);
        collectible1Holder = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getCollectible1Holder();
        collectible1Number = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getCollectible1Text();
    }

    /// <summary>
    /// Updates the HUD objects with current data.
    /// </summary>
    private void Update()
    {
        UpdateCollectibleCounter();
    }

    private void UpdateCollectibleCounter()
    {
        noCollectibles1 = FindObjectOfType<GameMaster>().getCollectible1();
        if (noCollectibles1.ToString().Equals("0"))
        {
            collectible1Holder.SetActive(false); ;
        }
        else
        {
            collectible1Holder.SetActive(true);
            collectible1Number.text = noCollectibles1.ToString();
        }
    }
}