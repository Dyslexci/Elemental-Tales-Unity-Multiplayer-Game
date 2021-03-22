using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Alan Zucconi
 *    @since 2.3.0
 *    @version 1.0.1
 *    
 *    Unused example of a 'safe float', where the float is stored in memory and cannot be modified by something like cheat engine. Would be used for protection. 
 *    Unused since we had no real reason for it - including for Security guys example.
 */
public struct SafeFloat
{
    private float offset;
    private float value;

    /// <summary>
    /// Constructs a new SafeFloat object with the value assigned.
    /// </summary>
    /// <param name="value"></param>
    public SafeFloat(float value = 0)
    {
        offset = Random.Range(-1000, +1000);
        this.value = value + offset;
    }

    /// <summary>
    /// Returns the correct float value.
    /// </summary>
    /// <returns></returns>
    public float GetValue()
    {
        return value - offset;
    }

    /// <summary>
    /// Removes the values of this object.
    /// </summary>
    public void Dispose()
    {
        offset = 0;
        value = 0;
    }

    /// <summary>
    /// Returns the correct value of this object as a string.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return GetValue().ToString();
    }

    /// <summary>
    /// Allows all + operators to be performed between SafeFloat objects.
    /// </summary>
    /// <param name="f1"></param>
    /// <param name="f2"></param>
    /// <returns></returns>
    public static SafeFloat operator +(SafeFloat f1, SafeFloat f2)
    {
        return new SafeFloat(f1.GetValue() + f2.GetValue());
    }

    /// <summary>
    /// Allows all - operators to be performed between SafeFloat objects.
    /// </summary>
    /// <param name="f1"></param>
    /// <param name="f2"></param>
    /// <returns></returns>
    public static SafeFloat operator -(SafeFloat f1, SafeFloat f2)
    {
        return new SafeFloat(f1.GetValue() - f2.GetValue());
    }
}
