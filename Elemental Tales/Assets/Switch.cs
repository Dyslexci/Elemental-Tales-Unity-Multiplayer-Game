using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static UnityEditor.PlayerSettings;
/*
 * @author Afoke Chizea
 * the script changes the animation of the
 */
public class Switch : MonoBehaviour
{
    // Start is called before the first frame update
   [SerializeField] GameObject crankDown;
   [SerializeField] GameObject crankUp;
   public bool isOn = false;
void Start()
{
    gameObject.GetComponent<SpriteRenderer>().sprite = crankDown.GetComponent<SpriteRenderer>().sprite;
}

// Update is called once per frame
void OnTriggerEnter2D(Collider2D col)
{
       
            gameObject.GetComponent<SpriteRenderer>().sprite = crankUp.GetComponent<SpriteRenderer>().sprite;
            isOn = true;
        
    
}
   

}