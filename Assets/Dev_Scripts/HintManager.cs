using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    public TextMeshProUGUI hintText;
    public Image mouseHintImage;
    public Sprite leftClickSprite;
    public Sprite rightClickSprite;
    public Sprite scrollSprite;
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;
    private bool hasMoved = false;
    private bool hasInteracted = false;
    private bool hasScrolled = false;

    void Start()
    {
        hintText.text = "";
        mouseHintImage.enabled = false;
        canvasGroup.alpha = 0f;
        Invoke(nameof(ShowMoveHint), 1f);
    }

    void Update()
    {
        if (!hasMoved && Input.GetMouseButtonDown(1))
        {
            hasMoved = true;
            StartCoroutine(FadeOut());
        }
    }

    public void ShowMoveHint()
    {
        if (!hasMoved)
        {
            hintText.text = "Right-click to move.";
            mouseHintImage.enabled = true;
            mouseHintImage.sprite = rightClickSprite;
            StartCoroutine(FadeIn());
        }
    }

    public void ShowInteractHint()
    {
        if (!hasInteracted)
        {
            hintText.text = "Left-click to interact.";
            mouseHintImage.enabled = true;
            mouseHintImage.sprite = leftClickSprite;
            StartCoroutine(FadeIn());
        }
    }

    public void ShowScrollHint()
    {
        if (!hasScrolled)
        {
            hintText.text = "Scroll out to see the whole village.";
            mouseHintImage.enabled = true;
            mouseHintImage.sprite = scrollSprite;
            StartCoroutine(FadeIn());
        }
    }

    public void HideScrollHint()
    {
        hasScrolled = true;
        StartCoroutine(FadeOut());
    }

    public void PlayerInteracted()
    {
        hasInteracted = true;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        hintText.text = "";
        mouseHintImage.enabled = false;
    }
}
