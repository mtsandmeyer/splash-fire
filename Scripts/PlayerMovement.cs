using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;

    public float moveSpeed;
    public float gravity = 9.81f;
    public float jumpHeight = 10f;
    public float airControl = 10f;

    private Vector3 moveDirection;
    private bool isFrozen = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFrozen)
        {
            return;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        var input = transform.right * moveHorizontal + transform.forward * moveVertical;

        input *= moveSpeed;

        if (controller.isGrounded)
        {
            moveDirection = input;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            }
        }
        else
        {
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }

        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);

    }

    public void FreezePlayer()
    {
        isFrozen = true;
    }
}
