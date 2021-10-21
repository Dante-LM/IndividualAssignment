using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform gunBarrel;
    public Transform bulletTracer;

    [SerializeField] float range = Mathf.Infinity;

    private void Awake()
    {
        
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
            {
                
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(gunBarrel.position, gunBarrel.TransformDirection(Vector3.forward) * range, Color.red);
    }
}
