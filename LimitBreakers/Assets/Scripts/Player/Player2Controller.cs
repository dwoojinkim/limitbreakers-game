using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
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
        playerScript.PlayerMovement(Input.GetAxis("Horizontal_2"));

        if (Input.anyKeyDown)
            playerScript.QuickFall(Input.GetAxis("Vertical_2"));

        if (Input.GetButtonDown("Jump_2"))
        {
            playerScript.IsJumpHeld = true;
            playerScript.Jump();
        }
        else if (Input.GetButtonUp("Jump_2"))
            playerScript.IsJumpHeld = false;

        if (Input.GetButtonDown("Action_2"))
            playerScript.PlayerActionPress();

        if (Input.GetButtonUp("Action_2"))
            playerScript.PlayerActionRelease();
    }
}
