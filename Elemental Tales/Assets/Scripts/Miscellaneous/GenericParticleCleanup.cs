using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 2.0.0
 *    @version 1.0.0
 *    
 *    Destroys the associated particle system once it has finished playing, to free up memory and increase performance.
 */
public class GenericParticleCleanup : MonoBehaviour
{
    ParticleSystem thisObject;

    void Start()
    {
        Debug.Log("Particle system created");
        thisObject = GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// Checks once a frame to see if this particle system is still running, and if not, destroys it.
    /// </summary>
    void Update()
    {
        if (!thisObject.IsAlive())
            Destroy(gameObject);
    }
}
