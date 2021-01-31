using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    Allows door with one or two assigned inputs to be opened permanently.
 */

public class doorLeverInput : MonoBehaviour
{
    [SerializeField] private Switch switch1;
    [SerializeField] private Switch switch2;

    public int switchNumber = 0;

    private void Start()
    {
        if (switch1 != null)
            switchNumber += 1;
        if (switch2 != null)
            switchNumber += 1;
    }

    void Update()
    {
        if (switchNumber == 0)
            return;

        if(switchNumber == 1)
        {
            if((switch1 != null && switch1.getLeverState() == true) || (switch2 != null && switch2.getLeverState() == true))
            {
                this.gameObject.SetActive(false);
                GameObject.Find("Game Manager").GetComponent<GameMaster>().openDoorSound.Play(0);
                if(switch1 != null)
                {
                    switch1.setPressedSuccessfully();
                } else if (switch2 != null)
                {
                    switch2.setPressedSuccessfully();
                }
            }
        } else if (switchNumber == 2)
        {
            if (switch1.getLeverState() == true && switch2.getLeverState() == true)
            {
                this.gameObject.SetActive(false);
                GameObject.Find("Game Manager").GetComponent<GameMaster>().openDoorSound.Play(0);
                switch1.setPressedSuccessfully();
                switch2.setPressedSuccessfully();
            }
        }
    }
}
