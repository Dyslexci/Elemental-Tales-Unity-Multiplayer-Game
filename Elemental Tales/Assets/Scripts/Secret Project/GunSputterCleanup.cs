using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSputterCleanup : MonoBehaviour
{
    ParticleSystem sputter;
    public float shipSpeed = 80;

    // Start is called before the first frame update
    void Start()
    {
        sputter = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!sputter.IsAlive())
            Destroy(gameObject);
    }
}
