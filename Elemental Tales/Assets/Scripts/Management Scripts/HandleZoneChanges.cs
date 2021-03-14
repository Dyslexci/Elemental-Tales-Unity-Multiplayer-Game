using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class HandleZoneChanges : MonoBehaviour
{
    public string areaEntering;
    public string areaNameString;

    public AudioSource forestMusic;
    public AudioSource poolsMusic;
    public AudioSource grottoMusic;

    [SerializeField] Transform pos;
    [SerializeField] float lengthX = 1.5f;
    [SerializeField] float lengthY = 1.5f;
    [SerializeField] private LayerMask layer;
    public float angle;

    TMP_Text areaNameText;
    CanvasGroup areaNamePanel;

    bool isPresent;
    bool wasPresent;

    float forestMusicVolume;
    float poolMusicVolume;
    float grottoMusicVolume;

    GameMaster gameMaster;

    private void Start()
    {
        gameMaster = GameObject.Find("Game Manager").GetComponent<GameMaster>();
        areaNameText = gameMaster.areaText;
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
        isPresent = false;
        checkPresent();

        if(isPresent && !wasPresent)
        {
            EnterNewArea(areaEntering);
        }
    }

    /// <summary>
    /// Checks for the players presence based off Physics2D collider circles and displays the hint if the player has entered the switch collider.
    /// </summary>
    private void checkPresent()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, new Vector2(lengthX, lengthY), angle, layer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player")
            {
                if (colliders[i].gameObject.GetPhotonView().IsMine)
                {
                    isPresent = true;

                    return;
                }
            }
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

    public void EnterNewArea(string areaName)
    {
        StartCoroutine(TransitionZoneText());

        if (areaName.Equals("Forest"))
        {
            if (gameMaster.inForest)
            {
                gameMaster.inPools = false;
                gameMaster.inGrotto = false;
                return;
            }
            else if (gameMaster.inPools)
            {
                gameMaster.inForest = true;
                gameMaster.inPools = false;
                gameMaster.inGrotto = false;
                StopAllCoroutines();
                forestMusic.Stop();
                poolsMusic.volume = poolMusicVolume;
                grottoMusic.Stop();
                StartCoroutine(TransitionZones(poolsMusic, forestMusic, poolMusicVolume, forestMusicVolume));
            }
            else if (gameMaster.inGrotto)
            {
                gameMaster.inForest = true;
                gameMaster.inPools = false;
                gameMaster.inGrotto = false;
                StopAllCoroutines();
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
                gameMaster.inForest = false;
                gameMaster.inGrotto = false;
                return;
            }
            else if (gameMaster.inForest)
            {
                gameMaster.inPools = true;
                gameMaster.inForest = false;
                gameMaster.inGrotto = false;
                StopAllCoroutines();
                forestMusic.volume = forestMusicVolume;
                poolsMusic.Stop();
                grottoMusic.Stop();
                StartCoroutine(TransitionZones(forestMusic, poolsMusic, forestMusicVolume, poolMusicVolume));
            }
            else if (gameMaster.inGrotto)
            {
                gameMaster.inPools = true;
                gameMaster.inForest = false;
                gameMaster.inGrotto = false;
                StopAllCoroutines();
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
                gameMaster.inForest = false;
                gameMaster.inPools = false;
                return;
            }
            else if (gameMaster.inForest)
            {
                gameMaster.inGrotto = true;
                gameMaster.inForest = false;
                gameMaster.inPools = false;
                StopAllCoroutines();
                forestMusic.volume = forestMusicVolume;
                poolsMusic.Stop();
                grottoMusic.Stop();
                StartCoroutine(TransitionZones(forestMusic, grottoMusic, forestMusicVolume, grottoMusicVolume));
            }
            else if (gameMaster.inPools)
            {
                gameMaster.inGrotto = true;
                gameMaster.inForest = false;
                gameMaster.inPools = false;
                StopAllCoroutines();
                forestMusic.Stop();
                poolsMusic.volume = poolMusicVolume;
                grottoMusic.Stop();
                StartCoroutine(TransitionZones(poolsMusic, grottoMusic, poolMusicVolume, grottoMusicVolume));
            }
        }
    }

    IEnumerator TransitionZoneText()
    {
        areaNamePanel.alpha = 1;
        while (areaNamePanel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            areaNamePanel.alpha -= .05f;
        }
        areaNameText.text = areaNameString;
        while (areaNamePanel.alpha < 1)
        {
            yield return new WaitForFixedUpdate();
            areaNamePanel.alpha += .05f;
        }
        areaNamePanel.alpha = 1;
    }

    IEnumerator TransitionZones(AudioSource fromMusic, AudioSource toMusic, float fromMusicStart, float toMusicStart)
    {
        toMusic.volume = 0;
        toMusic.Play(0);
        while(fromMusic.volume > 0)
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
