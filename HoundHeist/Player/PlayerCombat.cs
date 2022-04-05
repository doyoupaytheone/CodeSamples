using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private GameObject swordProjectile;

    private Transform playerTrans;
    private List<GameObject> projectilePool = new List<GameObject>();
    private Quaternion lastClickAngle = Quaternion.identity;

    private void Awake() => playerTrans = GetComponent<Transform>();

    private void Start()
    {
        InitializeWeapon(swordProjectile);
    }

    private void Update()
    {
        if (Input.GetKeyDown(PlayerPrefData.PrimaryAttack)) Attack();
        if (Input.GetKeyDown(PlayerPrefData.SecondaryAttack)) SecondaryAttack();
    }

    private void InitializeWeapon(GameObject projectilePrefab)
    {
        GameObject bullet;
        for (int i = 0; i < 12; i++)
        {
            bullet = CreateBullet(projectilePrefab);
            bullet.SetActive(false);
        }
    }

    private void Attack()
    {
        //Finds the direction of the mouse click, calculates the angle between it and the player and makes a proper rotation
        var mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var angle = Mathf.Atan2(mp.y - playerTrans.position.y, mp.x - playerTrans.position.x) * 180 / Mathf.PI; 
        lastClickAngle = Quaternion.Euler(0, 0, angle);

        //Creates the projectile
        FireWeapon();
    }

    private void SecondaryAttack()
    {

    }

    private GameObject CreateBullet(GameObject bulletPrefab)
    {
        //Creates a new bullet at  the bullet spawner and grabs it's bullet controller script
        GameObject projectile = Instantiate(bulletPrefab, playerTrans);

        //Adds the bullets to a pooled list to activate/deactivate and returns that bullet
        projectilePool.Add(projectile);
        return projectile;
    }

    private void FireWeapon()
    {
        //If the weapon is fired, turn on the next bullet in the pool
        foreach (GameObject projectile in projectilePool)
        {
            if (projectile.activeSelf == false)
            {
                projectile.transform.position = playerTrans.position;
                projectile.transform.rotation = lastClickAngle;
                projectile.GetComponent<ProjectileMovement>().ResetFirePosition();
                projectile.SetActive(true);

                break;
            }
        }
    }
}
