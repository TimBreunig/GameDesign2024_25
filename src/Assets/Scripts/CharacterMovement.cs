using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;

    [SerializeField] private float speed = 40f;
    [SerializeField] private float horizontalMove = 0f;

    private bool jump = false;

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        if(Input.GetButtonDown("Jump") )
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}
