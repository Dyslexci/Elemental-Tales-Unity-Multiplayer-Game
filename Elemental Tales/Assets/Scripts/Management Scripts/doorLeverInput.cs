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

    private int switchNumber = 0;

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
            }
        } else if (switchNumber == 2)
        {
            if (switch1.getLeverState() == true && switch2.getLeverState() == true)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
