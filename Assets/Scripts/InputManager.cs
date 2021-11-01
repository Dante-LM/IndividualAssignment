using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameObject[] weapons = new GameObject[2];

    [SerializeField] Movement movement;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] Gun gun;

    PlayerControls controls;
    PlayerControls.GroundMovementActions groundMovement;

    Vector2 horizontalInput;
    Vector2 mouseInput;

    Coroutine fireCoroutine;
    private bool isFiring;
    private bool canSwap;

    private void Awake()
    {
        weapons = GameObject.FindGameObjectsWithTag("PlayerWeapon");
        weapons[1].SetActive(false);
        controls = new PlayerControls();
        groundMovement = controls.GroundMovement;

        groundMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();

        groundMovement.Jump.performed += _ => movement.OnJumpPressed();

        groundMovement.PrimaryWeapon.performed += _ => EquipPrimary();
        groundMovement.SecondaryWeapon.performed += _ => EquipSecondary();
        groundMovement.Reload.performed += _ => gun.Reload();
        groundMovement.Pickup.performed += _ => PickUp();

        groundMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        groundMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

        groundMovement.Shoot.started += _ => StartFiring();
        groundMovement.Shoot.canceled += _ => StopFiring();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDestroy()
    {
        controls.Disable();
    }

    void Start()
    {
        if (weapons[0].activeInHierarchy)
        {
            gun = weapons[0].GetComponentInChildren<Gun>();
        }
        else if (weapons[1].activeInHierarchy)
        {
            gun = weapons[1].GetComponentInChildren<Gun>();
        }
    }

    void Update()
    {
        movement.ReceiveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);
    }

    void StartFiring()
    {
        gun.isFiring = true;
        isFiring = true;
        //Flash(true);
        fireCoroutine = StartCoroutine(gun.RapidFire());        
    }

    void StopFiring()
    {
        gun.isFiring = false;
        isFiring = false;
        //Flash(false);
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
    }

    //void Flash(bool onOrOff)
    //{
    //    gun.MuzzleFlash(onOrOff);
    //}

    void EquipPrimary()
    {
        canSwap = gun.WeaponSwap();
        if (weapons[1].activeSelf && !(isFiring || canSwap))
        {
            gun = weapons[0].GetComponentInChildren<Gun>();
            weapons[0].SetActive(true);
            weapons[1].SetActive(false);
            gun.readyAudio.PlayOneShot(gun.readyAudio.clip);
        }
    }

    void EquipSecondary()
    {
        canSwap = gun.WeaponSwap();
        if (weapons[0].activeSelf && !(isFiring || canSwap))
        {
            gun = weapons[1].GetComponentInChildren<Gun>();
            weapons[1].SetActive(true);
            weapons[0].SetActive(false);
            gun.readyAudio.PlayOneShot(gun.readyAudio.clip);
        }
    }

    void PickUp()
    {
        RaycastHit vision;

        if (Physics.Raycast(gun.gunBarrel.position, gun.gunBarrel.forward, out vision, 2f))
        {
            if (vision.collider.tag == "Gun")
            {
                GameObject newGun = vision.collider.GetComponent<GunPickup>().playerModel;
                Instantiate(newGun, gun.transform.parent.position, gun.transform.rotation, gun.transform.parent.parent);

                if (weapons[0].activeInHierarchy)
                {
                    Destroy(weapons[0]);
                    weapons = GameObject.FindGameObjectsWithTag("PlayerWeapon");
                    Debug.Log(weapons[0]);
                    weapons[0].SetActive(true);
                    gun = weapons[0].GetComponentInChildren<Gun>();
                    gun.gunBarrel = gun.gunBarrel = GameObject.FindWithTag("barrel").transform;
                    gun.anim = gun.GetComponentInParent<Animator>();
                }
                else if (weapons[1].activeInHierarchy)
                {
                    Destroy(weapons[1]);
                    weapons[1] = newGun;
                    gun = weapons[1].GetComponentInChildren<Gun>();
                    gun.gunBarrel = gun.gunBarrel = GameObject.FindWithTag("barrel").transform;
                    gun.anim = gun.GetComponentInParent<Animator>();
                }

                ////weapons = GameObject.FindGameObjectsWithTag("PlayerWeapon");
                //Debug.Log(weapons[0]);
                //Debug.Log(weapons[1]);
                //gun = weapons[0].GetComponentInChildren<Gun>();
                //Debug.Log(gun);
                ////gun.anim = gun.GetComponentInParent<Animator>();
                //gun.anim = weapons[0].GetComponent<Animator>();
                //gun.gunBarrel = GameObject.FindWithTag("barrel").transform;
                //Debug.Log(gun.anim);
                //weapons[0].SetActive(true);
                //weapons[1].SetActive(false);
                ////EquipSecondary();
            }
        }


    }
}
