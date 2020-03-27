using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    public int StartingStock = 5;
    public GameObject player1, player2, weapon;
    public GameObject p1StockTextObject, p2StockTextObject;

    private PlayerScript p1Script, p2Script;
    private TextMeshPro p1StockText, p2StockText;
    private Vector3 p1StartPos, p2StartPos, weaponStartPos;

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

        p1StartPos = player1.transform.position;
        p2StartPos = player2.transform.position;
        weaponStartPos = weapon.transform.position;

        p1Stock = StartingStock;
        p2Stock = StartingStock;

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

        if (Input.GetKeyDown("r"))
        {
            ResetGame();
        }
    }

    public void ReduceStock(string player)
    {
        if (player == "Player1")
            p1Stock--;
        else if (player == "Player2")
            p2Stock--;

        if (p1Stock <= 0 || p2Stock <= 0)
            ResetGame();
    }

    public void ResetGame()
    {
        //Resetting General game stuff
        Debug.Log("Resetting Game...");
        weapon.GetComponent<Weapon>().Reset();
        weapon.transform.position = weaponStartPos;

        //Resetting Player specific stuff
        p1Script.Reset();
        p2Script.Reset();

        p1Stock = StartingStock;
        p2Stock = StartingStock;

        player1.transform.position = p1StartPos;
        player2.transform.position = p2StartPos;
    }
}
