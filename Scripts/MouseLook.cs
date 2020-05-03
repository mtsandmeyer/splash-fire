using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;
    public float mouseSensitivity = 10f;

    private float xRotation = 0;

    void Start()
    {
        if (playerBody == null)
        {
            playerBody = transform.parent.transform;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= moveY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80);

        playerBody.Rotate(Vector3.up * moveX);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
