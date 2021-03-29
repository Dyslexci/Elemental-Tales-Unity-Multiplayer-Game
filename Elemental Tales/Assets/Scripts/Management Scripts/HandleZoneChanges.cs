using System.Collections;
using TMPro;
using UnityEngine;

/**
 *    @author Matthew Ahearn
 *    @since 2.2.0
 *    @version 1.2.2
 *
 *    Changes the current zone of the player, swapping music and changing the locator text to read the active location.
 */

public class HandleZoneChanges : CheckPresentController
{
    public string areaEntering;
    public string areaNameString;

    public AudioSource forestMusic;
    public AudioSource poolsMusic;
    public AudioSource grottoMusic;

    [SerializeField] private Transform pos;
    [SerializeField] private float lengthX = 1.5f;
    [SerializeField] private float lengthY = 1.5f;
    [SerializeField] private LayerMask layer;
    public float angle;

    private TMP_Text areaNameText;
    private CanvasGroup areaNamePanel;

    private bool isPresent;
    private bool wasPresent;

    private float forestMusicVolume;
    private float poolMusicVolume;
    private float grottoMusicVolume;

    private GameMaster gameMaster;

    private void Start()
    {
        gameMaster = GameObject.Find("Game Manager").GetComponent<GameMaster>();
        areaNameText = gameMaster.areaText;
        areaNamePanel = gameMaster.areaTextPanel;
        forestMusic = gameMaster.forestMusic;
        poolsMusic = gameMaster.poolsMusic;
        grottoMusic = gameMaster.grottoMusic;
        forestMusicVolume = forestMusic.volume;
        poolMusicVolume = poolsMusic.volume;
        grottoMusicVolume = grottoMusic.volume;
    }

    private void FixedUpdate()
    {
        wasPresent = isPresent;
        isPresent = CheckPresentBox(pos, lengthX, lengthY, angle, layer);

        if (isPresent && !wasPresent)
        {
            EnterNewArea(areaEntering);
        }
    }

    /// <summary>
    /// Draws a cisual representation of the switch collider in the editor.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, new Vector3(lengthX, lengthY, 1));
    }

    /// <summary>
    /// Refactored by Adnan
    /// initialises area and checks weather to start/stop coroutine
    /// </summary>
    /// <param name="inF"></param>
    /// <param name="inG"></param>
    /// <param name="inP"></param>
    /// <param name="stopCo"></param>
    private void switchArea(bool inF, bool inG, bool inP, bool stopCo)
    {
        gameMaster.inForest = inF;
        gameMaster.inGrotto = inG;
        gameMaster.inPools = inP;

        if (stopCo)
            StopAllCoroutines();
    }

    /// <summary>
    /// Refactored by Adnan
    /// Changes the music and text to refer to the new area.
    /// </summary>
    /// <param name="areaName"></param>
    public void EnterNewArea(string areaName)
    {
        areaNameText.text = areaNameString;
        if (areaName.Equals("Forest"))
        {
            if (gameMaster.inForest)
            {
                switchArea(true, false, false, false);
                return;
            }
            if (gameMaster.inPools)
            {
                switchArea(true, false, false, true);
                forestMusic.Stop();
                poolsMusic.volume = poolMusicVolume;
                grottoMusic.Stop();
                StartCoroutine(TransitionZones(poolsMusic, forestMusic, poolMusicVolume, forestMusicVolume));
            }
            else if (gameMaster.inGrotto)
            {
                switchArea(true, false, false, true);
                forestMusic.Stop();
                poolsMusic.Stop();
                grottoMusic.volume = grottoMusicVolume;
                StartCoroutine(TransitionZones(grottoMusic, forestMusic, grottoMusicVolume, forestMusicVolume));
            }
        }
        if (areaName.Equals("Pools"))
        {
            if (gameMaster.inPools)
            {
                switchArea(false, false, true, false);
                return;
            }
            if (gameMaster.inForest)
            {
                switchArea(false, false, true, true);
                forestMusic.volume = forestMusicVolume;
                poolsMusic.Stop();
                grottoMusic.Stop();
                StartCoroutine(TransitionZones(forestMusic, poolsMusic, forestMusicVolume, poolMusicVolume));
            }
            else if (gameMaster.inGrotto)
            {
                switchArea(false, false, true, true);
                forestMusic.Stop();
                poolsMusic.Stop();
                grottoMusic.volume = grottoMusicVolume;
                StartCoroutine(TransitionZones(grottoMusic, poolsMusic, grottoMusicVolume, poolMusicVolume));
            }
        }
        if (areaName.Equals("Grotto"))
        {
            if (gameMaster.inGrotto)
            {
                switchArea(false, true, false, false);
                return;
            }
            if (gameMaster.inForest)
            {
                switchArea(false, true, false, true);
                forestMusic.volume = forestMusicVolume;
                poolsMusic.Stop();
                grottoMusic.Stop();
                StartCoroutine(TransitionZones(forestMusic, grottoMusic, forestMusicVolume, grottoMusicVolume));
                return;
            }
            if (gameMaster.inPools)
            {
                switchArea(false, true, false, true);
                forestMusic.Stop();
                poolsMusic.volume = poolMusicVolume;
                grottoMusic.Stop();
                StartCoroutine(TransitionZones(poolsMusic, grottoMusic, poolMusicVolume, grottoMusicVolume));
            }
        }
    }

    /// <summary>
    /// Abstract method to change from some music, to some other music.
    /// </summary>
    /// <param name="fromMusic"></param>
    /// <param name="toMusic"></param>
    /// <param name="fromMusicStart"></param>
    /// <param name="toMusicStart"></param>
    /// <returns></returns>
    private IEnumerator TransitionZones(AudioSource fromMusic, AudioSource toMusic, float fromMusicStart, float toMusicStart)
    {
        toMusic.volume = 0;
        toMusic.Play(0);
        while (fromMusic.volume > 0)
        {
            yield return new WaitForFixedUpdate();
            fromMusic.volume -= .008f * fromMusicStart;
            toMusic.volume += .008f * toMusicStart;
        }
        fromMusic.volume = 0;
        toMusic.volume = toMusicStart;
        fromMusic.Stop();
        Debug.LogWarning("Now playing: " + toMusic.name);
    }
}