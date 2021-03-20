using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

/** 
 *    @author Matthew Ahearn
 *    @since 2.2.0
 *    @version 1.2.0
 *    
 *    Changes the current zone of the player, swapping music and changing the locator text to read the active location.
 */
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
        isPresent = false;
        checkPresent();

        if(isPresent && !wasPresent)
        {
            EnterNewArea(areaEntering);
        }
    }

    /// <summary>
    /// Refactored by Adnan
    /// Used Enhanced for loop
    /// Checks for the players presence based off Physics2D collider circles and displays the hint if the player has entered the switch collider.
    /// </summary>
    private void checkPresent()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, new Vector2(lengthX, lengthY), angle, layer);

        foreach (Collider2D c in colliders)
        {
            if (c.gameObject.tag == "Player")
            {
                if (c.gameObject.GetPhotonView().IsMine)
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


	/// <summary>
	/// Changes the music and text to refer to the new area.
	/// </summary>
	/// <param name="areaName"></param>
	public void EnterNewArea(string areaName)
	{
		this.areaNameText.text = this.areaNameString;
		if (areaName.Equals("Forest"))
		{
			if (this.gameMaster.inForest)
			{
				this.gameMaster.inPools = false;
				this.gameMaster.inGrotto = false;
				return;
			}
			if (this.gameMaster.inPools)
			{
				this.gameMaster.inForest = true;
				this.gameMaster.inPools = false;
				this.gameMaster.inGrotto = false;
				base.StopAllCoroutines();
				this.forestMusic.Stop();
				this.poolsMusic.volume = this.poolMusicVolume;
				this.grottoMusic.Stop();
				base.StartCoroutine(this.TransitionZones(this.poolsMusic, this.forestMusic, this.poolMusicVolume, this.forestMusicVolume));
			}
			else if (this.gameMaster.inGrotto)
			{
				this.gameMaster.inForest = true;
				this.gameMaster.inPools = false;
				this.gameMaster.inGrotto = false;
				base.StopAllCoroutines();
				this.forestMusic.Stop();
				this.poolsMusic.Stop();
				this.grottoMusic.volume = this.grottoMusicVolume;
				base.StartCoroutine(this.TransitionZones(this.grottoMusic, this.forestMusic, this.grottoMusicVolume, this.forestMusicVolume));
			}
		}
		if (areaName.Equals("Pools"))
		{
			if (this.gameMaster.inPools)
			{
				this.gameMaster.inForest = false;
				this.gameMaster.inGrotto = false;
				return;
			}
			if (this.gameMaster.inForest)
			{
				this.gameMaster.inPools = true;
				this.gameMaster.inForest = false;
				this.gameMaster.inGrotto = false;
				base.StopAllCoroutines();
				this.forestMusic.volume = this.forestMusicVolume;
				this.poolsMusic.Stop();
				this.grottoMusic.Stop();
				base.StartCoroutine(this.TransitionZones(this.forestMusic, this.poolsMusic, this.forestMusicVolume, this.poolMusicVolume));
			}
			else if (this.gameMaster.inGrotto)
			{
				this.gameMaster.inPools = true;
				this.gameMaster.inForest = false;
				this.gameMaster.inGrotto = false;
				base.StopAllCoroutines();
				this.forestMusic.Stop();
				this.poolsMusic.Stop();
				this.grottoMusic.volume = this.grottoMusicVolume;
				base.StartCoroutine(this.TransitionZones(this.grottoMusic, this.poolsMusic, this.grottoMusicVolume, this.poolMusicVolume));
			}
		}
		if (areaName.Equals("Grotto"))
		{
			if (this.gameMaster.inGrotto)
			{
				this.gameMaster.inForest = false;
				this.gameMaster.inPools = false;
				return;
			}
			if (this.gameMaster.inForest)
			{
				this.gameMaster.inGrotto = true;
				this.gameMaster.inForest = false;
				this.gameMaster.inPools = false;
				base.StopAllCoroutines();
				this.forestMusic.volume = this.forestMusicVolume;
				this.poolsMusic.Stop();
				this.grottoMusic.Stop();
				base.StartCoroutine(this.TransitionZones(this.forestMusic, this.grottoMusic, this.forestMusicVolume, this.grottoMusicVolume));
				return;
			}
			if (this.gameMaster.inPools)
			{
				this.gameMaster.inGrotto = true;
				this.gameMaster.inForest = false;
				this.gameMaster.inPools = false;
				base.StopAllCoroutines();
				this.forestMusic.Stop();
				this.poolsMusic.volume = this.poolMusicVolume;
				this.grottoMusic.Stop();
				base.StartCoroutine(this.TransitionZones(this.poolsMusic, this.grottoMusic, this.poolMusicVolume, this.grottoMusicVolume));
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
