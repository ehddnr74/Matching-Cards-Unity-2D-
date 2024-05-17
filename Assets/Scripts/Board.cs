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
        // ī�帶�� ������ȣ�� �ο��ϱ� ���� ����Ʈ �����
        CreateCardID();

        // ����Ʈ�� ���´�.
        ShuffleCardID();

        //���� �ʱ�ȭ
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
        // �� ��ҿ� ���� ������ �ε����� �����Ͽ� �� ��Ҹ� ��ȯ�Ѵ�. (cardIDList�� ��ҵ��� �����ϰ� ����)
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
        int rowCount = 6; // ����
        int colCount = 4; // ���� 

        int cardIndex = 0;

        Vector3 startPosition = new Vector3(-7.68f, 3.25f, 0f); // �� ���� �� ī�� ���� 

        float xSpacing = 1.71f; // x������ 1.71f�� ����
        float ySpacing = 2.09f; // y������ 2.09f�� ���� 

        // 24�� for���� ���鼭 6x4�� ī�� ��ġ (���ι������� �켱 ��ġ)
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
               card.SetCardID(cardID);// ī�忡 ID �ο� 
               card.SetMonsterSprite(cardSprites[cardID]); // ī��ID�� �´� ��������Ʈ�� ����
               cardList.Add(card); 
            }
        }
    }

    public List<Card> GetCards() { return cardList; }
}
