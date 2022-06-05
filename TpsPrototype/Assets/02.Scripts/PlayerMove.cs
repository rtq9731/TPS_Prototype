using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float mouseSensitivity = 5f;

    CharacterController ch = null;
    Animator anim = null;

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
        move.x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        move.z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

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
