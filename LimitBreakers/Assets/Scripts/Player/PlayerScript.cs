using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public GameObject debugTextObject;
    private TextMeshPro debugText;
    private GameManager gameManager;

    private Rigidbody2D rb2D;
    private float movementSpeed = 10.0f;
    private float jumpPower = 25.0f;
    private float jumpDrag = 10.0f;
    private float fallMultiplier = 5.0f;
    private float lowJumpMultiplier = 75.0f;
    private float quickFallMultiplier = 50.0f;
    private float xSpeed = 0.0f;

    private bool canJump = true, canDoubleJump = true;
    private bool canPickupWeapon = false;
    private bool isQuickFalling = false;
    private bool isOnGround = false;
    private bool isOnLeftWall = false, isOnRightWall = false;
    private bool isOnLeftSide = true;

    private Sprite leftSprite, rightSprite;

    private Transform equippedWeapon = null;
    private Transform equippableWeapon = null;
    private Vector3 updatedPosition;

    public bool IsJumpHeld { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        debugText = debugTextObject.GetComponent<TextMeshPro>();
        debugText.text = "";

        rb2D = gameObject.GetComponent<Rigidbody2D>();

        leftSprite = Resources.Load<Sprite>("Characters/BoxFighter_Left");
        rightSprite = Resources.Load<Sprite>("Characters/BoxFighter_Right");
    }

    // Update is called once per frame
    void Update()
    {
        //rb2D.velocity = (Vector2.right * xSpeed) + (Vector2.up * rb2D.velocity.y);
        transform.position += Vector3.right * xSpeed * Time.deltaTime;

        if (isQuickFalling)
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (quickFallMultiplier - 1) * Time.deltaTime;
        else
        {
            if (rb2D.velocity.y < 0)
                rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            else if (rb2D.velocity.y > 0)
            {
                if (IsJumpHeld)
                    rb2D.velocity += Vector2.up * Physics2D.gravity.y * (jumpDrag - 1) * Time.deltaTime;
                else
                    rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }

    public void PlayerMovement(float horizontalAxis)
    {
        xSpeed = horizontalAxis * movementSpeed;

        if (isOnLeftWall && xSpeed < 0)
            xSpeed = 0;
        else if (isOnRightWall && xSpeed > 0)
            xSpeed = 0;
    }

    public void QuickFall(float verticalAxis)
    {
            if (verticalAxis < 0 && !isOnGround && !isQuickFalling)
            {
                //Quick fall code
                if (rb2D.velocity.y > 0)
                    rb2D.velocity = Vector2.right * rb2D.velocity.x;

                isQuickFalling = true;
                canDoubleJump = false;
            }
    }

    public void Jump()
    {
        if (canJump || canDoubleJump)
        {
            rb2D.velocity = Vector2.up * jumpPower;
            if (canJump)
                canJump = false;
            else
                canDoubleJump = false;
            //debugText.text = "Jump";
        }
    }

    public void PlayerActionPress()
    {
        if (canPickupWeapon)
            PickupWeapon();
        else if (!canPickupWeapon && equippedWeapon != null)
            equippedWeapon.GetComponent<Weapon>().Charging = true;
    }

    public void PlayerActionRelease()
    {
        if (!canPickupWeapon && equippedWeapon != null && equippedWeapon.GetComponent<Weapon>().Charging)
        {
            ThrowWeapon();
        }
    }

    private void PickupWeapon()
    {
        equippedWeapon = equippableWeapon;
        equippedWeapon.GetComponent<Rigidbody2D>().simulated = false;
        equippedWeapon.parent = this.transform;
        if (isOnLeftSide)
            equippedWeapon.GetComponent<Weapon>().SwitchWeaponSide("right");   //Weapon is FACING right side
        else
            equippedWeapon.GetComponent<Weapon>().SwitchWeaponSide("left");   //Weapon is FACING left side

        canPickupWeapon = false;
    }

    private void ThrowWeapon()
    {
        equippedWeapon.GetComponent<Rigidbody2D>().simulated = true;
        equippedWeapon.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        equippedWeapon.parent = null;
        equippedWeapon.GetComponent<Weapon>().Throw(gameObject.layer);
        equippedWeapon = null;
    }

    public void KillPlayer()
    {
        Debug.Log(gameObject.name + " has been killed!");
        gameManager.ReduceStock(LayerMask.LayerToName(gameObject.layer));
    }

    //TODO: Switching sides can be inherited from a more basic unit since weapons can also switch sides
    public void SwitchPlayerSide()
    {
        if (isOnLeftSide)
            SwitchPlayerSide("right");
        else
            SwitchPlayerSide("left");
    }

    public void SwitchPlayerSide(string side)
    {
        if (side == "right")
        {
            isOnLeftSide = false;

            transform.GetComponent<SpriteRenderer>().sortingLayerName = "RightPlayer";
            transform.GetComponent<SpriteRenderer>().sprite = rightSprite;

            if (equippedWeapon != null)
                equippedWeapon.GetComponent<Weapon>().SwitchWeaponSide("left");   //Weapon is FACING left side
        }
        else if (side == "left")
        {
            isOnLeftSide = true;

            transform.GetComponent<SpriteRenderer>().sortingLayerName = "LeftPlayer";
            transform.GetComponent<SpriteRenderer>().sprite = leftSprite;

            if (equippedWeapon != null)
                equippedWeapon.GetComponent<Weapon>().SwitchWeaponSide("right");   //Weapon is FACING right side
        }
        
    }

    public void Respawn()
    {
        //Respawn logic here
    }

    public void Reset()
    {
        //Logic to reset the state of the player
        Debug.Log("Resetting " + this.name + "...");
        rb2D.velocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Ground":
                canJump = true;
                canDoubleJump = true;
                isOnGround = true;
                isQuickFalling = false;
                //debugText.text = "Ground";
                break;
            case "Wall":
                if (col.transform.position.x < this.transform.position.x)
                    isOnLeftWall = true;
                else
                    isOnRightWall = true;
                break;

        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Ground":
                isOnGround = false;
                break;
            case "Wall":
                if (isOnLeftWall)
                    isOnLeftWall = false;
                if (isOnRightWall)
                    isOnRightWall = false;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "WeaponTrigger")
        {
            canPickupWeapon = true;
            equippableWeapon = col.gameObject.transform.parent;

            if (col.gameObject.transform.parent.GetComponent<Weapon>().Ownership != 0 && col.gameObject.transform.parent.GetComponent<Weapon>().Ownership != gameObject.layer)
            {
                FindObjectOfType<HitStop>().Stop(0.05f);
                KillPlayer();
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "WeaponTrigger")
        {
            canPickupWeapon = false;
            equippableWeapon = null;
        }
    }
}
