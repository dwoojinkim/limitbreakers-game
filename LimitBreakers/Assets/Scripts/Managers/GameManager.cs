using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    public GameObject player1, player2;
    public GameObject p1StockTextObject, p2StockTextObject;

    private PlayerScript p1Script, p2Script;
    private TextMeshPro p1StockText, p2StockText;

    private int p1Stock, p2Stock;

    private bool isP1OnLeftSide;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        p1Script = player1.GetComponent<PlayerScript>();
        p2Script = player2.GetComponent<PlayerScript>();

        p1StockText = p1StockTextObject.GetComponent<TextMeshPro>();
        p2StockText = p2StockTextObject.GetComponent<TextMeshPro>();

        p1Stock = 5;
        p2Stock = 5;

        if (player1.transform.position.x < player2.transform.position.x)
        {
            p1Script.SwitchPlayerSide("left");
            p2Script.SwitchPlayerSide("right");

            isP1OnLeftSide = true;
        }
        else
        {
            p1Script.SwitchPlayerSide("right");
            p2Script.SwitchPlayerSide("left");

            isP1OnLeftSide = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((isP1OnLeftSide && player1.transform.position.x >= player2.transform.position.x) ||
            (!isP1OnLeftSide && player1.transform.position.x < player2.transform.position.x))
        {
            p1Script.SwitchPlayerSide();
            p2Script.SwitchPlayerSide();

            isP1OnLeftSide = !isP1OnLeftSide;
        }

        p1StockText.text = p1Stock.ToString();
        p2StockText.text = p2Stock.ToString();
    }

    public void ReduceStock(string player)
    {
        if (player == "Player1")
            p1Stock--;
        else if (player == "Player2")
            p2Stock--;
    }
}
