using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool IsActive { get; set; }
    public bool Charging { get; set; }
    public bool IsFacingRight {get; set; }
    public int Ownership { get; set; }

    private float baseThrowSpeed = 20.0f;
    private float chargeSpeed = 0.0f;
    private float maxChargeSpeed = 50.0f;
    private float chargeRate = 25.0f;

    private WeaponSpawner weaponSpawner;

    private Rigidbody2D weaponRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        Ownership = 0;

        weaponRigidbody = transform.GetComponent<Rigidbody2D>();

        Debug.Log(GameObject.Find("WeaponSpawner").name);
        weaponSpawner = GameObject.Find("WeaponSpawner").GetComponent<WeaponSpawner>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            if (Charging && chargeSpeed < maxChargeSpeed)
            {
                chargeSpeed += chargeRate * Time.deltaTime;
                if (chargeSpeed > maxChargeSpeed)
                    chargeSpeed = maxChargeSpeed;
            }
        }
    }

    public void Throw(int playerLayer)
    {
        Charging = false;
        if (IsFacingRight)
            weaponRigidbody.velocity += Vector2.right * (baseThrowSpeed + chargeSpeed);
        else
            weaponRigidbody.velocity -= Vector2.right * (baseThrowSpeed + chargeSpeed);
        chargeSpeed = 0.0f;
        Ownership = playerLayer;
    }

    //TODO: Switching sides can be inherited from a more basic unit since players can also switch sides
    public void SwitchWeaponSide()
    {
        if (IsFacingRight)
            SwitchWeaponSide("left");
        else
            SwitchWeaponSide("right");
    }

    public void SwitchWeaponSide(string facing)
    {
        if (facing == "left")
        {
            IsFacingRight = false;

            transform.localPosition = new Vector3(-0.5f, 0, 0);
            transform.localRotation = Quaternion.Euler(0, 180, 90);

        }
        else if (facing == "right")
        {
            IsFacingRight = true;

            transform.localPosition = new Vector3(0.5f, 0, 0);
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        
    }

    public void Reset()
    {
        Charging = false;
        chargeSpeed = 0.0f;
        weaponRigidbody.velocity = Vector2.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        weaponRigidbody.constraints = RigidbodyConstraints2D.None;
        Ownership = 0;
    }

    public void SpawnWeapon()
    {
        transform.GetComponent<BoxCollider2D>().enabled = true;
        transform.GetComponent<SpriteRenderer>().enabled = true;
        IsActive = true;
        Reset();
    }

    public void DestroyWeapon()
    {
        transform.GetComponent<BoxCollider2D>().enabled = false;
        transform.GetComponent<SpriteRenderer>().enabled = false;
        IsActive = false;
        weaponSpawner.RemoveWeapon();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
            Ownership = 0;
    }
}
