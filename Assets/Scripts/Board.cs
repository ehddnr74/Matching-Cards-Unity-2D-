using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private Sprite[] cardSprites;

    private List<int> cardIDList = new List<int>(); 
    private List<Card> cardList = new List<Card>();

    // Start is called before the first frame update
    void Start()
    {
        // 카드마다 고유번호를 부여하기 위한 리스트 만들기
        CreateCardID();

        // 리스트를 섞는다.
        ShuffleCardID();

        //보드 초기화
        InitBoard();
    }

    void CreateCardID()
    {
        for (int i = 0; i < cardSprites.Length; i++) 
        {
            // 0, 0, 1, 1, 2, 2, 3, 3, ... 11, 11
            cardIDList.Add(i);
            cardIDList.Add(i);
        }
    }

    void ShuffleCardID()
    {
        // 각 요소에 대해 랜덤한 인덱스를 선택하여 두 요소를 교환한다. (cardIDList의 요소들이 랜덤하게 섞임)
        for (int i = 0; i < cardIDList.Count; i++)
        {
            int randomIndex = Random.Range(0, cardIDList.Count);
            int temp = cardIDList[i];
            cardIDList[i] = cardIDList[randomIndex];
            cardIDList[randomIndex] = temp;
        }
    }

    void InitBoard()
    {
        int rowCount = 6; // 가로
        int colCount = 4; // 세로 

        int cardIndex = 0;

        Vector3 startPosition = new Vector3(-7.68f, 3.25f, 0f); // 맨 왼쪽 윗 카드 기준 

        float xSpacing = 1.71f; // x축으로 1.71f씩 증가
        float ySpacing = 2.09f; // y축으로 2.09f씩 감소 

        // 24번 for문을 돌면서 6x4의 카드 배치 (가로방향으로 우선 배치)
        for (int col = 0; col < colCount; col++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                Vector3 pos = new Vector3(
                     startPosition.x + row * xSpacing,
                     startPosition.y - col * ySpacing,
                     startPosition.z
                 );
               GameObject cardObj = Instantiate(cardPrefab, pos, Quaternion.identity);
               Card card = cardObj.GetComponent<Card>();
               int cardID = cardIDList[cardIndex++];
               card.SetCardID(cardID);// 카드에 ID 부여 
               card.SetMonsterSprite(cardSprites[cardID]); // 카드ID에 맞는 스프라이트로 설정
               cardList.Add(card); 
            }
        }
    }

    public List<Card> GetCards() { return cardList; }
}
