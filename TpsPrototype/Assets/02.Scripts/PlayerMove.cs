using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float jumpPower = 20f;
    [SerializeField] float sprintmultiplier = 2f;
    [SerializeField] float mouseSensitivity = 5f;

    float gravity = -9.8f;

    CharacterController ch = null;
    GroundTrigger groundTrigger = null;
    Animator anim = null;

    float originSpeed = 0f;
    float curSpeed = 0f;

    Vector3 move = Vector3.zero;
    Vector3 playerVelocity = Vector3.zero;
    Vector3 lastMouseInput = Vector3.zero;

    bool isJump = false;

    private void Awake()
    {
        ch = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        groundTrigger = GetComponentInChildren<GroundTrigger>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    // Update is called once per frame
    void Update()
    {
        GetPlayerKeyInput();
        SetPlayerMove();
        SetPlayerVelocity();
        CameraMove();
        SetAnim();
    }

    private void GetPlayerKeyInput()
    {

        if (Input.GetKeyDown(KeyCode.Space) && groundTrigger.isGround)
        {
            isJump = true;
            anim.SetTrigger("isJump");
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (curSpeed == speed)
                curSpeed *= sprintmultiplier;
        }
        else
        {
            curSpeed = speed;
        }
    }

    private void SetPlayerMove()
    {
        move = Vector3.zero;

        move += Input.GetAxis("Horizontal") * transform.right * curSpeed;
        move += Input.GetAxis("Vertical") * transform.forward * curSpeed;

        ch.Move(move * Time.deltaTime);
    }

    private void SetPlayerVelocity()
    {
        if (groundTrigger.isGround && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if(isJump)
        {
            playerVelocity.y = 0f;
            playerVelocity.y += jumpPower;
            isJump = false;
        }

        playerVelocity.y += gravity * Time.deltaTime;

        ch.Move(playerVelocity * Time.deltaTime);
    }

    private void CameraMove()
    {
        Vector3 deltaMouseMove = Vector3.zero;

        deltaMouseMove.y = Input.GetAxis("Mouse X");

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + deltaMouseMove * mouseSensitivity);
    }

    private void SetAnim()
    {
        anim.SetBool("isGround", groundTrigger.isGround);
        anim.SetFloat("Speed", Mathf.Lerp(0.75f, 1.25f, curSpeed / (speed * sprintmultiplier)));

        anim.SetBool("isMove", move.x != 0 || move.z != 0);

        anim.SetFloat("HorizontalMove", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("FowardMove", Input.GetAxisRaw("Vertical"));
    }
}
