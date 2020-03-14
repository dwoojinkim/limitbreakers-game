using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool Charging { get; set; }
    public int Ownership { get; set; }

    private float baseThrowSpeed = 20.0f;
    private float chargeSpeed = 0.0f;
    private float maxChargeSpeed = 75.0f;
    private float chargeRate = 35.0f;

    // Start is called before the first frame update
    void Start()
    {
        Ownership = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Charging && chargeSpeed < maxChargeSpeed)
        {
            chargeSpeed += chargeRate * Time.deltaTime;
            if (chargeSpeed > maxChargeSpeed)
                chargeSpeed = maxChargeSpeed;
        }
    }

    public void Throw(int playerLayer)
    {
        Charging = false;
        transform.GetComponent<Rigidbody2D>().velocity += Vector2.right * (baseThrowSpeed + chargeSpeed);
        chargeSpeed = 0.0f;
        Ownership = playerLayer;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
            Ownership = 0;
    }
}
