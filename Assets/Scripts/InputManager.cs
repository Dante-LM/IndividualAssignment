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

    private Animator anim;

    private void Awake()
    {

        anim = weapons[1].GetComponent<Animator>();
        if (weapons[0].activeInHierarchy)
        {
            gun = weapons[0].GetComponentInChildren<Gun>();
            //anim = weapons[0].GetComponent<Animator>();
        }            
        else
        {
            gun = weapons[1].GetComponentInChildren<Gun>();
            //anim = weapons[1].GetComponent<Animator>();
        }


        controls = new PlayerControls();
        groundMovement = controls.GroundMovement;

        groundMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();

        groundMovement.Jump.performed += _ => movement.OnJumpPressed();

        groundMovement.PrimaryWeapon.performed += _ => EquipPrimary();
        groundMovement.SecondaryWeapon.performed += _ => EquipSecondary();
        groundMovement.Reload.performed += _ => Reload();

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
        
    }

    void Update()
    {
        movement.ReceiveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);
    }

    void StartFiring()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("draw"))
        {
            isFiring = true;
            Flash(true);
            anim.Play("shoot_not_empty");
            fireCoroutine = StartCoroutine(gun.RapidFire());
        }            
    }

    void StopFiring()
    {
        isFiring = false;
        Flash(false);
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
    }

    void Flash(bool onOrOff)
    {
        gun.MuzzleFlash(onOrOff);
    }

    void EquipPrimary()
    {
        if (!isFiring)
        {
            anim.Play("holster");
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("holster"))
            {
                gun = weapons[0].GetComponentInChildren<Gun>();
                weapons[0].SetActive(true);
                weapons[1].SetActive(false);
            }                
        }            
    }

    void EquipSecondary()
    {
        if (!isFiring)
        {
            gun = weapons[1].GetComponentInChildren<Gun>();
            weapons[1].SetActive(true);
            weapons[0].SetActive(false);
        }            
    }

    void Reload()
    {
        anim.Play("reload_not_empty");
    }
}
