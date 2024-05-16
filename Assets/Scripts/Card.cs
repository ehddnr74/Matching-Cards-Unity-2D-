using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer cardRenderer;

    [SerializeField]
    private Sprite monsterSprite;

    [SerializeField]
    private Sprite backSprite;

    // isFlipped가 false라면 카드 뒷면을 보여주고 true라면 카드 앞면을 보여준다.
    private bool isFlipped = false;
    private bool isMatched = false;

    // Card Scaleing Animation
    private float scaleDuration = 0.2f;

    // 카드를 뒤집는 도중인지 체크하기 위한 변수 
    private bool isFlipping = false;

    public int cardID;

    public bool GetIsMatched() { return isMatched; }
    public int GetCardID() { return cardID; }
    public void SetCardID(int id) { cardID = id; }
    public void SetMatched() { isMatched = true; }
    public void SetMonsterSprite(Sprite sprite) { this.monsterSprite = sprite; }

    public void FlipCard()
    {
        // Start the flip animation coroutine
        StartCoroutine(FlipCardAnimation());
    }

    private IEnumerator FlipCardAnimation()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(0f, originalScale.y, originalScale.z);

        // 카드 스케일 줄여서 뒤집어지는 것처럼 표현 
        yield return StartCoroutine(ScaleOverTime(originalScale, targetScale, scaleDuration));

        //  isFlipped가 false인 상태가 뒷면 앞면이면 뒷면으로 뒷면이면 앞면으로 변경  
        isFlipped = !isFlipped;
        if (isFlipped)
            cardRenderer.sprite = monsterSprite;
        else
            cardRenderer.sprite = backSprite;

        // 카드가 뒤집히고 나면 다시 스케일 늘림
        yield return StartCoroutine(ScaleOverTime(targetScale, originalScale, scaleDuration));
        isFlipping = false;
    }

    private IEnumerator ScaleOverTime(Vector3 fromScale, Vector3 toScale, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // 시작 스케일부터 목표스케일까지 't'값에 따라 부드럽게 보간되어 변환 
            transform.localScale = Vector3.Lerp(fromScale, toScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = toScale;
    }


    void OnMouseDown()
    {
        if (!isFlipping && !isMatched && !isFlipped && !GameManager.instance.GetIsMatching())
        {
            isFlipping = true;
            GameManager.instance.ClickCard(this);
        }
    }
}
