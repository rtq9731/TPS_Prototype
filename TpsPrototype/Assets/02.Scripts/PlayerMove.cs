using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float sprintmultiplier = 2f;
    [SerializeField] float mouseSensitivity = 5f;

    CharacterController ch = null;
    Animator anim = null;

    float originSpeed = 0f;
    float curSpeed = 0f;

    Vector3 move = Vector3.zero;
    Vector3 lastMouseInput = Vector3.zero;

    private void Awake()
    {
        ch = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (curSpeed == speed)
                curSpeed *= sprintmultiplier;
        }
        else
        {
            curSpeed = speed;
        }

        anim.SetFloat("Speed", Mathf.Lerp(0.75f, 1.25f, curSpeed / (speed * sprintmultiplier)));

        move.x = Input.GetAxis("Horizontal") * Time.deltaTime * curSpeed;
        move.z = Input.GetAxis("Vertical") * Time.deltaTime * curSpeed;

        anim.SetBool("isMove", move != Vector3.zero);

        anim.SetFloat("HorizontalMove", Input.GetAxis("Horizontal"));
        anim.SetFloat("FowardMove", Input.GetAxis("Vertical"));

        ch.Move(move.z * transform.forward);
        ch.Move(move.x * transform.right);
        ch.Move(new Vector3(0f, -9.8f, 0f));

        Vector3 deltaMouseMove = Vector3.zero;

        deltaMouseMove.y = Input.GetAxis("Mouse X");

        transform.rotation =Quaternion.Euler(transform.rotation.eulerAngles + deltaMouseMove * mouseSensitivity);
    }
}
