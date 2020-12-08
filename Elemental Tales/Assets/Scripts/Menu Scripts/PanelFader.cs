using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFader : MonoBehaviour
{
    private bool mFaded;
    [SerializeField] private float m_Duration = 0.4f;

    public void setFaded()
    {
        mFaded = true;
        GetComponent<CanvasGroup>().alpha = 0;
    }

    public void setUnfaded()
    {
        mFaded = false;
        GetComponent<CanvasGroup>().alpha = 1;
    }

    public void fade(GameObject currentScene)
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(DoFade(canvasGroup, canvasGroup.alpha, mFaded ? 1 : 0, currentScene));

        mFaded = !mFaded;
    }

    IEnumerator DoFade(CanvasGroup canvGroup, float start, float end, GameObject scene)
    {
        float counter = 0f;
        while (counter < m_Duration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / m_Duration);
            
        }
        if (end == 0)
            scene.SetActive(false);
        yield return null;
    }

}
