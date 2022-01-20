using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Flash : MonoBehaviorHelper<Flash>
{
    [Header("# Flash Image #")]
    public Image img_Flash;

    [Header("Settings")]
    public float flashColorAlpha = 0.5f;

    private void Awake()
    {
        Clear();
    }

    private void Clear()
    {
        Color flashColor = img_Flash.color;
        flashColor.a = flashColorAlpha;
        img_Flash.color = flashColor;
        img_Flash.gameObject.SetActive(false);
    }

    public void DoFlash(Action OnCompleted = null)
    {
        Clear();

        img_Flash.gameObject.SetActive(true);

        StartCoroutine(DoFade(img_Flash, flashColorAlpha, 0, flashColorAlpha, () =>
        {
            OnCompleted?.Invoke();
        }));
    }

    private IEnumerator DoFade(Image img, float from, float to, float time, Action OnCompleted)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            Color c = img.color;
            float f = Mathf.Lerp(from, to, (elapsedTime / time));
            c.a = f;
            img.color = c;

            elapsedTime += Time.deltaTime;
            elapsedTime = Mathf.Clamp01(elapsedTime);

            yield return 0;
        }

        Color cc = img.color;
        cc.a = to;
        img.color = cc;

        yield return 0;

        if (to == 0)
        {
            img.gameObject.SetActive(false);
        }

        yield return 0;

        OnCompleted?.Invoke();
    }
}
