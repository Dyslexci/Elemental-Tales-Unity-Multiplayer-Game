using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Scroll());
    }

    IEnumerator Scroll()
    {
        while(true)
        {
            yield return new WaitForFixedUpdate();
            transform.position = new Vector2(transform.position.x - .03f, transform.position.y);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
