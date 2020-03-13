using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controller : MonoBehaviour
{
    private PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = this.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        playerScript.PlayerMovement(Input.GetAxis("Horizontal"));

        if (Input.anyKeyDown)
            playerScript.QuickFall(Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Jump"))
            playerScript.Jump();

        if (Input.GetButtonDown("Action"))
            playerScript.PlayerActionPress();

        if (Input.GetButtonUp("Action"))
            playerScript.PlayerActionRelease();
    }
}
