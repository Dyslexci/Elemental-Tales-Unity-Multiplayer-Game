using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFader : MonoBehaviour
{
    private bool mFaded = true;
    [SerializeField] private float m_Duration = 0.4f;

    public void fade()
    {
        var canvGroup = GetComponent<CanvasGroup>();
        StartCoroutine(DoFade(canvGroup, canvGroup.alpha, mFaded ? 1 : 0));
        mFaded = !mFaded;
    }

    public IEnumerator DoFade(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;
        while(counter < m_Duration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / m_Duration);
            
        }
        yield return null;
    }
}
