using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    Allows door with two assigned inputs to be opened permanently.
 */

public class doorOneInput : MonoBehaviour
{
    [SerializeField] private Switch switch1;

    void Update()
    {
        if (switch1.getLeverState() == true)
        {
            this.gameObject.SetActive(false);
        }
        else if (switch1.getLeverState() == false)
        {
            this.gameObject.SetActive(true);
        }
    }
}
