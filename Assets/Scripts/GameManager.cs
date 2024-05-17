using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<Card> Cards;

    private Card flippedCard;

    private bool isFlipping = false; 
    private bool isMatching = false;

    private bool ShowAgain = true; // ī�� �ٽú��⸦ ���� bool ���� 
    private bool isGameOver = false; // GameOver bool ���� 

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private TextMeshProUGUI gameOverText;


    [SerializeField]
    private TextMeshProUGUI clearText;

    [SerializeField]
    private TextMeshProUGUI clearTimeText;

    [SerializeField]
    private Slider timeSlider;
    [SerializeField]
    private float timeLimit = 60f; // ���� �ð� 

    private float currentTime; // ���� ���� ���� �ð�
    private float startTime;

    private int matchCnt = 0; // ��Ī �� ī��� �� 

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

        currentTime = timeLimit;
        SetTimeText();
        StartCoroutine(FlipCard());
    }

    void SetTimeText() // Ÿ�̸��� ��, �� ��� 
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private IEnumerator FlipCard() // ������ �� �����ֱ� ���� ī�� ������
    {
        isFlipping = true;

        yield return new WaitForSeconds(1.0f);
        FlipCards();
        yield return new WaitForSeconds(3f);
        FlipCards();
        yield return new WaitForSeconds(0.5f);


        isFlipping = false;
        ShowAgain = false;

        yield return StartCoroutine(Timer()); // �ڷ�ƾ�� ������ Ÿ�̸Ӹ� ����
    }

    private IEnumerator Timer() // Ÿ�̸�
    {
        while(currentTime >= 0 && !isGameOver)
        {
            if (currentTime <= 0.1f && !isGameOver)
            {
                GameOver(true);
                break;
            }
            currentTime -= Time.deltaTime;
            timeSlider.value = currentTime / timeLimit;
            SetTimeText();
            yield return null;
        }



    }

    public void FlipCards()
    {
        foreach (Card card in Cards)
        {
            card.FlipCard();
        }

    }

    public void ShowAgainOnebyOne() // �� �徿 ����� ī�带 �����ִ� �ڷ�ƾ
    {
        StartCoroutine(ShowCards());
    }

    private IEnumerator ShowCards()
    {
        isFlipping = true;

        // ��Ī���� ���� ī�带 �ϳ��� ����� ������
        foreach (Card card in Cards)
        {
            if (!card.GetIsMatched())
            {
                card.FlipCard(); 
                yield return new WaitForSeconds(0.4f); 
                card.FlipCard(); 
                yield return new WaitForSeconds(0.4f); 
            }
        }

        isFlipping = false;
        ShowAgain = false;
    }

    public void ShowAgainAll() // �ѹ��� ī�带 �ٽ� ����� �����ִ� �ڷ�ƾ 
    {
        StartCoroutine(ShowCardsAll());
    }

    private IEnumerator ShowCardsAll()
    {
        isFlipping = true;

        List<Card> unmatchedCards = new List<Card>();

        // ����Ʈ�� ��Ī���� ���� ī��鸸 �ְ� �����´�.
        foreach (Card card in Cards)
        {
            if (!card.GetIsMatched())
            {
                unmatchedCards.Add(card); 
                card.FlipCard(); 
            }
        }

        yield return new WaitForSeconds(1f); 

        // ��� ��Ī���� ���� ī�带 �ٽ� �����´�.
        foreach (Card card in unmatchedCards)
        {
            card.FlipCard(); 
        }

        isFlipping = false;
        ShowAgain = false;
    }

    public void ClickCard(Card card)
    {
        if(isFlipping || isGameOver)
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

            matchCnt++;
            if (matchCnt == Cards.Count / 2)
            {
                GameOver(false);
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);

            card1.FlipCard();
            card2.FlipCard();

            yield return new WaitForSeconds(0.4f);
        }

        isFlipping = false;
        flippedCard = null;
        isMatching = false;
    }

    void GameOver(bool gameover)
    {
        if (!isGameOver)
        {
            isGameOver = true;

            StopCoroutine(Timer());

            if (gameover)
                gameOverText.SetText("Fail");
            else
            {
                clearText.SetText("Clear Time : ");

                float elapsedTime = timeLimit - currentTime;
                int minutes = Mathf.FloorToInt(elapsedTime / 60);
                int seconds = Mathf.FloorToInt(elapsedTime % 60);

                clearTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                gameOverText.SetText("Clear");

                clearText.gameObject.SetActive(true);
                clearTimeText.gameObject.SetActive(true);
            }
      

            Invoke("ShowGameOverUI", 1f);
        }
    }


    void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    public void ReTry()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
