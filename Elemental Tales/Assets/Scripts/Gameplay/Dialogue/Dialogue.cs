using UnityEngine;

/**
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *
 *    Stores dialogue input from unity until called into the dialogue manager.
 */

[System.Serializable]
public class Dialogue
{
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;
}