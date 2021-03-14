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
    private static bool level1TimeHasChanged = false;
    private static int level1Stage;
    private static float level1BestTime;
    private static float level1Stage1BestTime;
    private static float level1Stage2BestTime;
    private static float level1Stage3BestTime;
    private static int previousStage;
    private static int playerDeaths;

    public static int PlayerDeaths
    {
        get
        {
            return playerDeaths;
        }
        set
        {
            playerDeaths = value;
        }
    }

    public static int PreviousStage
    {
        get
        {
            return previousStage;
        }
        set
        {
            previousStage = value;
        }
    }

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

    public static int Level1Stage
    {
        get
        {
            return level1Stage;
        }
        set
        {
            level1Stage = value;
        }
    }

    public static float Level1BestTime
    {
        get
        {
            return level1BestTime;
        }
        set
        {
            level1BestTime = value;
        }
    }

    public static float Level1Stage1BestTime
    {
        get
        {
            return level1Stage1BestTime;
        }
        set
        {
            level1Stage1BestTime = value;
        }
    }
    public static float Level1Stage2BestTime
    {
        get
        {
            return level1Stage2BestTime;
        }
        set
        {
            level1Stage2BestTime = value;
        }
    }
    public static float Level1Stage3BestTime
    {
        get
        {
            return level1Stage3BestTime;
        }
        set
        {
            level1Stage3BestTime = value;
        }
    }

    public static bool Level1TimeHasChanged
    {
        get
        {
            return level1TimeHasChanged;
        }
        set
        {
            level1TimeHasChanged = value;
        }
    }
}
