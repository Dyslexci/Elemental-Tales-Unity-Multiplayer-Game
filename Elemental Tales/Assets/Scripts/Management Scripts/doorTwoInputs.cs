using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    Allows door with two assigned inputs to be opened permanently. Now deprecated by @doorLeverInput.cs.
 */

public class doorTwoInputs : MonoBehaviour
{
    [SerializeField] private Switch switch1;
    [SerializeField] private Switch switch2;
    private GameObject gameMaster;

    /// <summary>
    /// Checks for state of both associated levers and opens the door when both are set to active.
    /// </summary>
    void Update()
    {
        if (switch1.getLeverState() && switch2.getLeverState())
        {
            this.gameObject.SetActive(false);
            GameObject.Find("Game Master").GetComponent<GameMaster>().openDoorSound.Play();
        }
    }
}
