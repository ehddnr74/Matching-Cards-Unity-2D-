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

    // isFlipped�� false��� ī�� �޸��� �����ְ� true��� ī�� �ո��� �����ش�.
    private bool isFlipped = false;
    private bool isMatched = false;

    // Card Scaleing Animation
    private float scaleDuration = 0.2f;

    // ī�带 ������ �������� üũ�ϱ� ���� ���� 
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

        // ī�� ������ �ٿ��� ���������� ��ó�� ǥ�� 
        yield return StartCoroutine(ScaleOverTime(originalScale, targetScale, scaleDuration));

        //  isFlipped�� false�� ���°� �޸� �ո��̸� �޸����� �޸��̸� �ո����� ����  
        isFlipped = !isFlipped;
        if (isFlipped)
            cardRenderer.sprite = monsterSprite;
        else
            cardRenderer.sprite = backSprite;

        // ī�尡 �������� ���� �ٽ� ������ �ø�
        yield return StartCoroutine(ScaleOverTime(targetScale, originalScale, scaleDuration));
        isFlipping = false;
    }

    private IEnumerator ScaleOverTime(Vector3 fromScale, Vector3 toScale, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // ���� �����Ϻ��� ��ǥ�����ϱ��� 't'���� ���� �ε巴�� �����Ǿ� ��ȯ 
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
