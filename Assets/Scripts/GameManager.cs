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

        // 매칭되지 않은 카드만 잠시 다시 보여줍니다.
        foreach (Card card in Cards)
        {
            if (!card.GetIsMatched())
            {
                card.FlipCard(); // 카드를 뒤집습니다.
                yield return new WaitForSeconds(0.4f); // 각 카드를 보여주는 시간을 조절할 수 있습니다.
                card.FlipCard(); // 카드를 다시 뒤집습니다.
                yield return new WaitForSeconds(0.4f); // 각 카드를 뒤집는 시간을 조절할 수 있습니다.
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

        // 매칭되지 않은 카드만 잠시 다시 보여줍니다.
        foreach (Card card in Cards)
        {
            if (!card.GetIsMatched())
            {
                unmatchedCards.Add(card); // 매칭되지 않은 카드를 리스트에 추가합니다.
                card.FlipCard(); // 각 카드를 뒤집습니다.
            }
        }

        yield return new WaitForSeconds(1f); // 매칭되지 않은 카드를 모두 보여준 후에 잠시 대기합니다.

        // 모든 매칭되지 않은 카드를 다시 뒤집습니다.
        foreach (Card card in unmatchedCards)
        {
            card.FlipCard(); // 각 카드를 다시 뒤집습니다.
        }

        //yield return new WaitForSeconds(1f); // 모든 매칭되지 않은 카드를 다시 뒤집은 후에 잠시 대기합니다.

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
