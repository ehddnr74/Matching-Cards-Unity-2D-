using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<Card> Cards;

    private Card flippedCard;

    private bool isFlipping = false;
    private bool isMatching = false;

    private bool ShowAgain = true;

    public void SetShowing(bool Showing) { ShowAgain = Showing; }
    public bool GetShowing() { return ShowAgain; }
    public Card GetFlippedCard() { return flippedCard; }
    public bool GetIsFlipping() { return isFlipping; }
    public bool GetIsMatching() { return isMatching; }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Board board = FindObjectOfType<Board>();
        Cards = board.GetCards();

        StartCoroutine(FlipCard());
    }

    private IEnumerator FlipCard()
    {
        isFlipping = true;

        yield return new WaitForSeconds(0.5f);
        FlipCards();
        yield return new WaitForSeconds(3f);
        FlipCards();
        yield return new WaitForSeconds(0.5f);


        isFlipping = false;
        ShowAgain = false;
    }

    public void FlipCards()
    {
        foreach (Card card in Cards)
        {
            card.FlipCard();
        }

    }

    public void ShowAgainOnebyOne()
    {
        StartCoroutine(ShowCards());
    }

    private IEnumerator ShowCards()
    {
        isFlipping = true;

        // ��Ī���� ���� ī�常 ��� �ٽ� �����ݴϴ�.
        foreach (Card card in Cards)
        {
            if (!card.GetIsMatched())
            {
                card.FlipCard(); // ī�带 �������ϴ�.
                yield return new WaitForSeconds(0.4f); // �� ī�带 �����ִ� �ð��� ������ �� �ֽ��ϴ�.
                card.FlipCard(); // ī�带 �ٽ� �������ϴ�.
                yield return new WaitForSeconds(0.4f); // �� ī�带 ������ �ð��� ������ �� �ֽ��ϴ�.
            }
        }

        isFlipping = false;
        ShowAgain = false;
    }

    public void ShowAgainAll()
    {
        StartCoroutine(ShowCardsAll());
    }

    private IEnumerator ShowCardsAll()
    {
        isFlipping = true;

        List<Card> unmatchedCards = new List<Card>();

        // ��Ī���� ���� ī�常 ��� �ٽ� �����ݴϴ�.
        foreach (Card card in Cards)
        {
            if (!card.GetIsMatched())
            {
                unmatchedCards.Add(card); // ��Ī���� ���� ī�带 ����Ʈ�� �߰��մϴ�.
                card.FlipCard(); // �� ī�带 �������ϴ�.
            }
        }

        yield return new WaitForSeconds(1f); // ��Ī���� ���� ī�带 ��� ������ �Ŀ� ��� ����մϴ�.

        // ��� ��Ī���� ���� ī�带 �ٽ� �������ϴ�.
        foreach (Card card in unmatchedCards)
        {
            card.FlipCard(); // �� ī�带 �ٽ� �������ϴ�.
        }

        //yield return new WaitForSeconds(1f); // ��� ��Ī���� ���� ī�带 �ٽ� ������ �Ŀ� ��� ����մϴ�.

        isFlipping = false;
        ShowAgain = false;
    }

    public void ClickCard(Card card)
    {
        if(isFlipping)
        {
            return;
        }

        card.FlipCard();

        if (flippedCard == null)
            flippedCard = card;
        else
            StartCoroutine(FindSamePicture(flippedCard, card));
    }

    IEnumerator FindSamePicture(Card card1, Card card2)
    {
        isMatching = true;
        isFlipping = true;

        if (card1.GetCardID() == card2.GetCardID())
        {
            card1.SetMatched();
            card2.SetMatched();
        }
        else
        {
            Debug.Log("Different Card");
            yield return new WaitForSeconds(1f);

            card1.FlipCard();
            card2.FlipCard();

            yield return new WaitForSeconds(0.4f);
        }

        isFlipping = false;
        flippedCard = null;
        isMatching = false;
    }
        
}
