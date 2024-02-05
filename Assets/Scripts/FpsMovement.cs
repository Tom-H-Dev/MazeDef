using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsMovement : MonoBehaviour
{
    [SerializeField] private Camera headCam;

    public float speed = 6.0f;
    private float gravity = -9.8f;

    private float sensitivityHor = 9.0f;
    private float sensitivityVert = 9.0f;

    private float minimumVert = -45.0f;
    private float maximumVert = 45.0f;

    private float rotationVert = 0;



    private Rigidbody _rb;
    private Animator _animator;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    

    void Update()
    {
        MoveCharacter();
        RotateCharacter();
        RotateCamera();
    }

    private void MoveCharacter()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);

        movement.y = gravity;
        //movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);

        _rb.velocity = movement;

        if (deltaX != 0 || deltaZ != 0)
            _animator.SetBool("isMoving", true);
        else _animator.SetBool("isMoving", false);
    }

    private void RotateCharacter()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
    }

    private void RotateCamera()
    {
        rotationVert -= Input.GetAxis("Mouse Y") * sensitivityVert;
        rotationVert = Mathf.Clamp(rotationVert, minimumVert, maximumVert);

        headCam.transform.localEulerAngles = new Vector3(rotationVert, headCam.transform.localEulerAngles.y, 0);
    }
}
