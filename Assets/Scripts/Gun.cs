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

    WaitForSeconds rapidFireWait;

    private void Awake()
    {
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        muzzleFlash.SetActive(false);
    }
    
    public void Shoot()
    {
        Instantiate(bulletTracer, gunBarrel.position, gunBarrel.rotation);
        RaycastHit hit;
        if(Physics.Raycast(gunBarrel.position, gunBarrel.forward, out hit, range))
        {
            if(hit.collider.GetComponent<Damageable>() != null)
            {
                hit.collider.GetComponent<Damageable>().TakeDamage(hit.point, hit.normal);
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
}
