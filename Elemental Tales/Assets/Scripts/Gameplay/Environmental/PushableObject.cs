using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CharacterControllerRaycast))]
public class PushableObject : MonoBehaviour
{

	public float gravity = 12;
	CharacterControllerRaycast controller;
	Vector3 velocity;
	string hintTooltip;
	TMP_Text hintText;
	GameObject hintHolder;
	CanvasGroup panel;
	Image hintImage;
	bool isDisplayingHint;

	void Start()
	{
		hintText = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintText();
		hintHolder = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().GetHintHolder();
		hintImage = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintContainer();
		controller = GetComponent<CharacterControllerRaycast>();
		panel = hintHolder.GetComponentInChildren<CanvasGroup>();
	}

	void FixedUpdate()
	{
		velocity += Vector3.down * gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime, false);
		if (controller.collisions.below)
		{
			velocity = Vector3.zero;
		}

		if (!controller.collisions.wasDisplayingHint && controller.collisions.shouldDisplayHint && isDisplayingHint == false)
        {
			isDisplayingHint = true;
			StartCoroutine("WaitHideHint");
		}
	}

	IEnumerator WaitHideHint()
    {
		GameObject.Find("Game Manager").GetComponent<GameMaster>().hintSound.Play(0);
		hintHolder.SetActive(true);
		StartCoroutine("JumpInHintHolder");
		hintText.text = "<color=#ffffff>Hold SHIFT to <color=#ffeb04> grab and move <color=#ffffff>objects!";
		yield return new WaitForSeconds(2);
		StartCoroutine("FadeHintHolder");
	}

	IEnumerator JumpInHintHolder()
    {
		hintImage.transform.localScale = new Vector3(5, 5, 5);

		while (hintImage.transform.localScale.x > 1)
        {
			yield return new WaitForFixedUpdate();
			hintImage.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
		}
		hintImage.transform.localScale = new Vector3(1, 1, 1);
	}

	IEnumerator FadeHintHolder()
    {
		while(panel.alpha > 0)
        {
			yield return new WaitForFixedUpdate();
			panel.alpha -= 0.05f;
		}
		
		hintHolder.SetActive(false);
		panel.alpha = 1;
		isDisplayingHint = false;
	}

	public Vector2 Push(Vector2 amount)
	{
		return controller.Move(amount, false);
	}
}