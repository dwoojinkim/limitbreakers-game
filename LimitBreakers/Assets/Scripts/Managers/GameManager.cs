using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    public GameObject player1;
    public GameObject player2;

    private PlayerScript p1Script;
    private PlayerScript p2Script;

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
    }
}
