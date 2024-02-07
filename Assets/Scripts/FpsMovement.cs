using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FpsMovement : MonoBehaviour
{
    [SerializeField] private Camera headCam;

    public float speed = 6.0f;
    private float gravity = -9.8f;
    public bool canMove = true;

    private float sensitivityHor = 9.0f;
    private float sensitivityVert = 9.0f;

    private float minimumVert = -45.0f;
    private float maximumVert = 45.0f;

    private float rotationVert = 0;

    [SerializeField] private GameObject _pauseMenu;

    private Rigidbody _rb;
    private Animator _animator;
    private AudioSource _audioSource;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
        if (_animator is null)
            Debug.LogError("Animator is " + _animator);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }



    void Update()
    {
        if (canMove)
        {
            MoveCharacter();
            RotateCharacter();
            RotateCamera();
        }
        OpenPauseMenu();
    }

    private void MoveCharacter()
    {
        //Input
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        //Movement calculation
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);

        movement.y = gravity;
        movement = transform.TransformDirection(movement);

        _rb.velocity = movement;

        //If the player is moving set the audio and the animations
        if (deltaX != 0 || deltaZ != 0)
        {
            _animator.SetBool("isMoving", true);
            _audioSource.Play();
        }
        else
        {
            _animator.SetBool("isMoving", false);
            _audioSource.Stop();
        }
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

    private void OpenPauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_pauseMenu.active)
            {
                _pauseMenu.SetActive(true);
                canMove = false;
                SetCursorSettingsMenu(true);
            }
            else
            {
                _pauseMenu.SetActive(false);
                canMove= true;
                SetCursorSettingsMenu(false);
            }
        }
    }

    public void ChangeCanMove(bool l_value)
    {
        canMove = l_value;
    }

    public void SetCursorSettingsMenu(bool l_openMenu)
    {
        if (l_openMenu)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Main Menu");
    }

    /// <summary>
    /// Emote is broken so function not included in game anymore
    /// </summary>
    private void EmoteWheel()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            _animator.SetTrigger("Dance");
        }
    }
    

}
