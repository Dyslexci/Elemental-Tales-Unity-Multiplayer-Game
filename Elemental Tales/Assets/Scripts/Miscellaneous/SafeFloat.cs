using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Alan Zucconi
 *    @since 2.3.0
 *    @version 1.0.0
 *    
 *    Unused example of a 'safe float', where the float is stored in memory and cannot be modified by something like cheat engine. Would be used for protection. 
 *    Unused since we had no real reason for it - including for Security guys example.
 */
public struct SafeFloat
{
    private float offset;
    private float value;
    public SafeFloat(float value = 0)
    {
        offset = Random.Range(-1000, +1000);
        this.value = value + offset;
    }

    public float GetValue()
    {
        return value - offset;
    }
    public void Dispose()
    {
        offset = 0;
        value = 0;
    }
    public override string ToString()
    {
        return GetValue().ToString();
    }
    public static SafeFloat operator +(SafeFloat f1, SafeFloat f2)
    {
        return new SafeFloat(f1.GetValue() + f2.GetValue());
    }

    public static SafeFloat operator -(SafeFloat f1, SafeFloat f2)
    {
        return new SafeFloat(f1.GetValue() - f2.GetValue());
    }
}
