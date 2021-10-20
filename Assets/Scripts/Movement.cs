using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 10f;
    Vector2 horizontalInput;

    [SerializeField] float jumpHeight = 2f;
    bool jump;

    [SerializeField] float gravity = -9.81f;
    Vector3 verticalVelocity = Vector3.zero;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;
    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }

    void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + new Vector3(0, -1.25f, 0), 0.2f);
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position + new Vector3(0, -1.25f, 0), 0.2f, groundMask);
        if (isGrounded)
        {
            verticalVelocity.y = 0;
        }

        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * speed;
        controller.Move(horizontalVelocity * Time.deltaTime);

        if(jump)
        {
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }
            jump = false;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }

    public void OnJumpPressed()
    {
        jump = true;
    }
}
