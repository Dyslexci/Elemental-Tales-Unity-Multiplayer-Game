using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericParticleCleanup : MonoBehaviour
{
    ParticleSystem thisObject;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Particle system created");
        thisObject = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!thisObject.IsAlive())
            Destroy(gameObject);
    }
}
