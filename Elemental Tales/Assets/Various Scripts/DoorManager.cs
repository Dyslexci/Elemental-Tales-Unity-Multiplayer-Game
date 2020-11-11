using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
/*
 * @author Afoke Chizea
 * @since0.0.0
 * @version 0.1.0
 * 
 * This script will open open doors on te map when 2 levers crank up
 */



public class DoorManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Switch lever1;
    [SerializeField] Switch lever2;
    public int noOfLevers = 0;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        getNoOfLevers();
        raiseDoor();


    }


    public void getNoOfLevers()
    {

        if (lever1.isOn && lever2.isOn)
        {
            noOfLevers++;
        }
        

        
        //return noOfLevers;
    }

    public void raiseDoor()
    {
        if(noOfLevers == 2 )
        {
            DestroyDoors();
        }
    }
   

    public void DestroyDoors()
    {
        gameObject.SetActive(false);
    }

    
}
