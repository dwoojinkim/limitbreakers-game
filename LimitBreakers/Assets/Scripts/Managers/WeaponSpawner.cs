using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    private List<GameObject> weaponPool;
    private float spawnTimer = 0.0f;
    private float timeToSpawn = 3.0f;
    private bool activeWeapon = false;

    // Start is called before the first frame update
    void Awake()
    {
        weaponPool = new List<GameObject>();

        GameObject sword = (GameObject)Instantiate(Resources.Load("Prefabs/Sword"), this.transform);

        weaponPool.Add(sword);
    }

    // Update is called once per frame
    void Update()
    {
        if (!activeWeapon)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= timeToSpawn)
            {
                spawnTimer = 0.0f;
                SpawnWeapon();
            }
        }
    }

    private void SpawnWeapon()
    {
        activeWeapon = true;

        weaponPool[0].transform.localPosition = Vector3.zero;
        weaponPool[0].GetComponent<Weapon>().SpawnWeapon();
    }

    public void RemoveWeapon()
    {
        activeWeapon = false;

        weaponPool[0].transform.parent = this.transform;
    }
}
