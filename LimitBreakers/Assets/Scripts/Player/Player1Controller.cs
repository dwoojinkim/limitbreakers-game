﻿using System.Collections;
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
        playerScript.PlayerMovement(Input.GetAxis("Horizontal_1"));

        if (Input.anyKeyDown)
            playerScript.QuickFall(Input.GetAxis("Vertical_1"));

        if (Input.GetButtonDown("Jump_1"))
        {
            playerScript.IsJumpHeld = true;
            playerScript.Jump();
        }
        else if (Input.GetButtonUp("Jump_1"))
            playerScript.IsJumpHeld = false;

        if (Input.GetButtonDown("Action_1"))
            playerScript.PlayerActionPress();

        if (Input.GetButtonUp("Action_1"))
            playerScript.PlayerActionRelease();
    }
}
