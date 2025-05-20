using UnityEngine;
using UnityEngine.UI;

public class DragHintVisual : MonoBehaviour
{
    public RectTransform cursorImage;
    public RectTransform boxImage;
    public Vector2 startOffset = new Vector2(0, 0);
    public Vector2 endOffset = new Vector2(150, -100);
    public float animationDuration = 1f;
    public float delayBetweenLoops = 0.5f;
    private Vector2 startPos;
    private Vector2 endPos;
    private float timer;

    void OnEnable()
    {
        ResetAnimation();
        StartCoroutine(LoopAnimation());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void ResetAnimation()
    {
        timer = 0f;
        startPos = startOffset;
        endPos = endOffset;
        cursorImage.anchoredPosition = startPos;
        boxImage.sizeDelta = Vector2.zero;
    }

    System.Collections.IEnumerator LoopAnimation()
    {
        while (true)
        {
            timer = 0f;

            while (timer < animationDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / animationDuration);
                cursorImage.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                boxImage.sizeDelta = new Vector2(
                    Mathf.Abs(cursorImage.anchoredPosition.x - startPos.x),
                    Mathf.Abs(cursorImage.anchoredPosition.y - startPos.y)
                );
                yield return null;
            }

            yield return new WaitForSeconds(delayBetweenLoops);
            ResetAnimation();
        }
    }
}
