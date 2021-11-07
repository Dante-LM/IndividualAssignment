using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Components")]
    public Transform gunBarrel;
    public Transform bulletTracer;
    public GameObject muzzleFlash;
    public Animator anim;

    [Header("Gun Variables")]
    [SerializeField] float range = 50f;
    [SerializeField] float fireRate = 5f;
    [SerializeField] bool rapidFire = false;
    WaitForSeconds rapidFireWait;
    [SerializeField] bool waitForShoot;
    private float inaccuracyModifier = 1;

    [SerializeField] int ammoMag;
    [SerializeField] int ammoTotal;

    [HideInInspector] public int currentAmmoMag;
    [HideInInspector] public int currentAmmoTotal;
    [HideInInspector] public bool emptyMag = false;
    [HideInInspector] public TextMeshProUGUI ammoText;

    [HideInInspector]
    public bool isFiring = false;
    private GameObject currentSceneManager;

    [Header("Shotgun")]
    [SerializeField] bool shotgun = false;
    [SerializeField] bool pump = false;
    [SerializeField] int pelletsPerShot = 6;
    [SerializeField] float pelletSpread;

    [Header("Audio")]
    [SerializeField] AudioSource fireAudio;
    [SerializeField] public AudioSource readyAudio;
    [SerializeField] AudioSource reloadAudio;
    [SerializeField] AudioSource emptyReloadAudio;

    private void Awake()
    {
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        muzzleFlash.SetActive(false);
        currentAmmoMag = ammoMag;
        currentAmmoTotal = ammoTotal;
    }

    public void Shoot()
    {
        if (waitForShoot)
        {
            if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("reload_not_empty") || anim.GetCurrentAnimatorStateInfo(0).IsName("reload_empty") || anim.GetCurrentAnimatorStateInfo(0).IsName("draw") || anim.GetCurrentAnimatorStateInfo(0).IsName("shoot_not_empty")))
            {
                CheckLastShot();

                if (shotgun)
                {
                    for (int i = 0; i < pelletsPerShot; i++)
                    {
                        RaycastDamage();
                    }
                }
                else
                {
                    RaycastDamage();
                    inaccuracyModifier += 0.25f;
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
                CheckLastShot();

                if (shotgun)
                {
                    for (int i = 0; i < pelletsPerShot; i++)
                    {
                        RaycastDamage();
                    }
                }
                else
                {
                    RaycastDamage();
                    inaccuracyModifier += 0.25f;
                }
                currentAmmoMag--;
                if (MagCheck())
                    Reload();
            }
        }
    }

    void RaycastDamage()
    {
        //Instantiate(bulletTracer, gunBarrel.position, gunBarrel.rotation);
        RaycastHit hit;

        if (Physics.Raycast(gunBarrel.position, GetShootingDirection(), out hit, range))
        {
            if (hit.collider.GetComponent<Damageable>() != null)
            {
                hit.collider.GetComponent<Damageable>().TakeDamage(hit.point, hit.normal);
            }
        }
    }

    void CheckLastShot()
    {
        if (currentAmmoMag > 1)
            anim.Play("shoot_not_empty");
        else
            anim.Play("shoot_empty");

        fireAudio.Stop();
        fireAudio.PlayOneShot(fireAudio.clip);

        if (pump)
        {
            StartCoroutine(WaitForSound(fireAudio.clip));
        }
    }

    public IEnumerator WaitForSound(AudioClip audio)
    {
        //yield return new WaitWhile(() => fireAudio.isPlaying == false);
        yield return new WaitForSeconds(fireAudio.clip.length - 1.25f);
        emptyReloadAudio.PlayOneShot(emptyReloadAudio.clip);
    }

    void Start()
    {
        gunBarrel = GameObject.FindWithTag("barrel").transform;
        ammoText = GameObject.FindWithTag("AmmoUI").GetComponent<TextMeshProUGUI>();
        readyAudio.PlayOneShot(readyAudio.clip);
        anim = GetComponentInParent<Animator>();
        currentSceneManager = GameObject.FindGameObjectWithTag("Button");
    }

    void Update()
    {
        AccuracyCheck();
        UpdateAmmo();
    }

    void UpdateAmmo()
    {
        ammoText.SetText(string.Format("{0}/{1}", currentAmmoMag, currentAmmoTotal));
    }

    void AccuracyCheck()
    {
        if (!isFiring)
        {
            inaccuracyModifier = 0.25f;
        }
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
        if(!(currentAmmoMag == ammoMag))
        {
            if (currentAmmoMag == 0)
            {
                anim.Play("reload_empty");
                if(shotgun)
                    reloadAudio.PlayOneShot(reloadAudio.clip);
                else
                    emptyReloadAudio.PlayOneShot(emptyReloadAudio.clip);
            }
            else
            {
                anim.Play("reload_not_empty");
                reloadAudio.PlayOneShot(reloadAudio.clip);
            }
            currentAmmoMag = ammoMag;
        }
        inaccuracyModifier = 0.25f;
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
    
    Vector3 GetShootingDirection()
    {
        if (shotgun)
        {
            Vector3 targetPos = gunBarrel.position + gunBarrel.forward * range;
            targetPos = new Vector3(
                targetPos.x + Random.Range(-pelletSpread, pelletSpread),
                targetPos.y + Random.Range(-pelletSpread, pelletSpread),
                targetPos.z + Random.Range(-pelletSpread, pelletSpread));

            Vector3 direction = targetPos - gunBarrel.position;
            return direction.normalized;
        }
        else
        {
            Vector3 targetPos = gunBarrel.position + gunBarrel.forward * range;
            targetPos = new Vector3(
                targetPos.x + Random.Range(-inaccuracyModifier, inaccuracyModifier),
                targetPos.y + Random.Range(-inaccuracyModifier, inaccuracyModifier),
                targetPos.z + Random.Range(-inaccuracyModifier, inaccuracyModifier));

            Vector3 direction = targetPos - gunBarrel.position;
            return direction.normalized;
        }
    }

    public void PressButton()
    {
        RaycastHit view;
        if (Physics.Raycast(gunBarrel.position, gunBarrel.forward, out view, 3f))
        {
            if (view.collider.gameObject.tag == "Button")
            {
                if (view.collider.gameObject.GetComponent<RangeManager>() != null)
                {
                    currentSceneManager = view.collider.gameObject;
                    if(!currentSceneManager.GetComponent<RangeManager>().isPlaying)
                        currentSceneManager.GetComponent<RangeManager>().StartPlaying();
                }
                else if (view.collider.gameObject.GetComponent<CourseManager>() != null)
                {
                    currentSceneManager = view.collider.gameObject;
                    if (!currentSceneManager.GetComponent<CourseManager>().isPlaying)
                        currentSceneManager.GetComponent<CourseManager>().StartPlaying();
                }
                else
                    Debug.Log("No button");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
    }
}
