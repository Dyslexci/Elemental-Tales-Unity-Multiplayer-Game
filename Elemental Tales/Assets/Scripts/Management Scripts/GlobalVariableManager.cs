using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 1.0.0
 *    
 *    Stores application wide variables which must not be unloaded during this session, but which can be unloaded between application runs.
 */

public static class GlobalVariableManager
{
    private static bool hasLoaded = false;

    public static bool HasLoaded
    {
        get
        {
            return hasLoaded;
        }
        set
        {
            hasLoaded = value;
        }
    }
}
