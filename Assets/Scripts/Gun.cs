using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform gunBarrel;
    public Transform bulletTracer;
    public GameObject muzzleFlash;

    [SerializeField] float range = Mathf.Infinity;
    [SerializeField] float fireRate = 5f;
    [SerializeField] bool rapidFire = false;
    [SerializeField] bool waitForShoot;
    [SerializeField] int ammoMag;
    private int currentAmmoMag;
    //[SerializeField] int ammoTotal;
    private bool emptyMag = false;

    WaitForSeconds rapidFireWait;

    [SerializeField] Animator anim;

    private void Awake()
    {
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        muzzleFlash.SetActive(false);
        currentAmmoMag = ammoMag;
    }

    public void Shoot()
    {
        if (waitForShoot)
        {
            if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("reload_not_empty") || anim.GetCurrentAnimatorStateInfo(0).IsName("reload_empty") || anim.GetCurrentAnimatorStateInfo(0).IsName("draw") || anim.GetCurrentAnimatorStateInfo(0).IsName("shoot_not_empty")))
            {
                anim.Play("shoot_not_empty");
                Instantiate(bulletTracer, gunBarrel.position, gunBarrel.rotation);
                RaycastHit hit;
                if (Physics.Raycast(gunBarrel.position, gunBarrel.forward, out hit, range))
                {
                    if (hit.collider.GetComponent<Damageable>() != null)
                    {
                        hit.collider.GetComponent<Damageable>().TakeDamage(hit.point, hit.normal);
                    }
                }
                currentAmmoMag--;
                if (MagCheck())
                    Reload();
            }
        }
        else
        {
            if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("reload_not_empty") || anim.GetCurrentAnimatorStateInfo(0).IsName("reload_empty") || anim.GetCurrentAnimatorStateInfo(0).IsName("draw")))
            {
                anim.Play("shoot_not_empty");
                Instantiate(bulletTracer, gunBarrel.position, gunBarrel.rotation);
                RaycastHit hit;
                if (Physics.Raycast(gunBarrel.position, gunBarrel.forward, out hit, range))
                {
                    if (hit.collider.GetComponent<Damageable>() != null)
                    {
                        hit.collider.GetComponent<Damageable>().TakeDamage(hit.point, hit.normal);
                    }
                }
                currentAmmoMag--;
                if (MagCheck())
                    Reload();
            }
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public IEnumerator RapidFire()
    {
        if (rapidFire)
        {
            while (true)
            {
                Shoot();
                yield return rapidFireWait;
            }
        }
        else
        {
            Shoot();
            yield return null;
        }
    }

    public void MuzzleFlash(bool onOrOff)
    {
        muzzleFlash.SetActive(onOrOff);
    }

    public void Reload()
    {
        if (currentAmmoMag == 0)
            anim.Play("reload_empty");
        else
            anim.Play("reload_not_empty");
        currentAmmoMag = ammoMag;
    }

    public bool WeaponSwap()
    {
        bool canSwap;

        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("draw") || anim.GetCurrentAnimatorStateInfo(0).IsName("reload_not_empty") || anim.GetCurrentAnimatorStateInfo(0).IsName("reload_empty")))
            canSwap = true;
        else
            canSwap = false;

        return canSwap;
    }
    public bool MagCheck()
    {
        if (currentAmmoMag == 0)
            emptyMag = true;
        else
            emptyMag = false;
        return emptyMag;
    }
}
