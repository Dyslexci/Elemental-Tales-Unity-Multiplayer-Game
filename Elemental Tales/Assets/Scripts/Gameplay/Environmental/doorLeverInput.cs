using UnityEngine;

/**
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 1.1.0
 *
 *    Allows door with one or two assigned inputs to be opened permanently.
 */

public class doorLeverInput : MonoBehaviour
{
    [SerializeField] private Switch switch1;
    [SerializeField] private Switch switch2;

    public int switchNumber = 0;
    public bool displayHint;

    private GameMaster gameMaster;

    /// <summary>
    /// Initialises the door, determining the number of switches allocated.
    /// </summary>
    private void Start()
    {
        if (switch1 != null)
            switchNumber += 1;
        if (switch2 != null)
            switchNumber += 1;

        gameMaster = GameObject.Find("Game Manager").GetComponent<GameMaster>();
    }

    /// <summary>
    /// Determines if enough switches are held down to trigger its end state, opening the door permanently if so.
    /// </summary>
    private void Update()
    {
        if (switchNumber == 0)
            return;

        if (switchNumber == 1)
        {
            CheckForOneSwitch();
        }
        else if (switchNumber == 2)
        {
            CheckForTwoSwitches();
        }
    }

    private void CheckForOneSwitch()
    {
        if ((switch1 != null && switch1.getLeverState()) || (switch2 != null && switch2.getLeverState()))
        {
            transform.position = new Vector2(10000, 10000);

            gameMaster.openDoorSound.Play(0);
            if (switch1 != null)
            {
                switch1.setPressedSuccessfully();
            }
            else if (switch2 != null)
            {
                switch2.setPressedSuccessfully();
            }
        }
    }

    private void CheckForTwoSwitches()
    {
        if (switch1.getLeverState() && switch2.getLeverState())
        {
            displayHint = false;
            transform.position = new Vector2(10000, 10000);
            gameMaster.openDoorSound.Play(0);
            switch1.setPressedSuccessfully();
            switch2.setPressedSuccessfully();
        }

        if (switch1.getLeverState() || switch2.getLeverState())
        {
            displayHint = true;
        }
        else if ((!switch1.getLeverState() && !switch2.getLeverState()) || (switch1.pressedSuccessfully && switch2.pressedSuccessfully))
        {
            displayHint = false;
        }
    }
}