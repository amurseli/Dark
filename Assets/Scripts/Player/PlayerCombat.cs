using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("AttackAttributes")]
    public float attackRange = 20f;
    public float shotSpeed = 10f;
    public float fireRate = 0.5f;

    public Transform gun;
    public GameObject bulletPrefab;

    private bool canShoot = true;
    

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && canShoot)
        {
            canShoot = false;
            var bullet = Instantiate(bulletPrefab, gun.position, gun.rotation);
            bullet.GetComponent<Rigidbody>().velocity = gun.forward * shotSpeed;
            StartCoroutine(bulletCooldown());
        }
    }

    IEnumerator bulletCooldown()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            //Debug.Log("HELL IS FOREEEVEEERRR");
        }
        
    }
}
