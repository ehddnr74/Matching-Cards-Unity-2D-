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

    private bool ShowAgain = true; // 카드 다시보기를 위한 bool 변수 
    private bool isGameOver = false; // GameOver bool 변수 

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
    private float timeLimit = 60f; // 제한 시간 

    private float currentTime; // 게임 도중 현재 시간
    private float startTime;

    private int matchCnt = 0; // 매칭 된 카드쌍 수 

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

    void SetTimeText() // 타이머의 분, 초 계산 
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private IEnumerator FlipCard() // 시작할 때 보여주기 위한 카드 뒤집기
    {
        isFlipping = true;

        yield return new WaitForSeconds(1.0f);
        FlipCards();
        yield return new WaitForSeconds(3f);
        FlipCards();
        yield return new WaitForSeconds(0.5f);


        isFlipping = false;
        ShowAgain = false;

        yield return StartCoroutine(Timer()); // 코루틴이 끝나면 타이머를 가동
    }

    private IEnumerator Timer() // 타이머
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

    public void ShowAgainOnebyOne() // 한 장씩 뒤집어서 카드를 보여주는 코루틴
    {
        StartCoroutine(ShowCards());
    }

    private IEnumerator ShowCards()
    {
        isFlipping = true;

        // 매칭되지 않은 카드를 하나씩 뒤집어서 보여줌
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

    public void ShowAgainAll() // 한번에 카드를 다시 뒤집어서 보여주는 코루틴 
    {
        StartCoroutine(ShowCardsAll());
    }

    private IEnumerator ShowCardsAll()
    {
        isFlipping = true;

        List<Card> unmatchedCards = new List<Card>();

        // 리스트에 매칭되지 않은 카드들만 넣고 뒤집는다.
        foreach (Card card in Cards)
        {
            if (!card.GetIsMatched())
            {
                unmatchedCards.Add(card); 
                card.FlipCard(); 
            }
        }

        yield return new WaitForSeconds(1f); 

        // 모든 매칭되지 않은 카드를 다시 뒤집는다.
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
