using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Animator anim;
    private float curSpeed;

    public float ratioForSensInZoom = .5f;
    public float moveSpeed = .05f;
    public float sensensity = 1f;
    public GameObject modPlayer;

    

    void Start()
    {
        anim = GetComponent<Animator>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        curSpeed = moveSpeed;
    }

	void Update ()
    {
        if (!GetComponent<UIControlling>().GetInInventory())
        {
            RotateCam();
            MoveCam();
        }
    }


    //Player Cam Rotation
    void RotateCam()
    {
        float rotatorX = Input.GetAxis("Mouse X") * sensensity;
        float rotatorY = Input.GetAxis("Mouse Y") * sensensity;

        transform.Rotate(0, rotatorX, 0);

        // Block rotation for camera or rotate normaly
        if (modPlayer.transform.localRotation.x <= .58f && -rotatorY >= 0f ||
            modPlayer.transform.localRotation.x >= -.70f && -rotatorY <= 0f)
        {
            modPlayer.transform.Rotate(-rotatorY, 0, 0);
        }
    }
    
    //Player Movements
    void MoveCam()
    {
        float moveHoriz = Input.GetAxis("Horizontal");
        float moveVertic = Input.GetAxis("Vertical");

        //Char Move
        if (moveHoriz != 0 || moveVertic != 0) 
        {
            anim.SetBool("Move", true);
        }
        else
            anim.SetBool("Move", false);


        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("Run", true);
            curSpeed = moveSpeed * 2f;
        }
        else
        {
            anim.SetBool("Run", false);
            curSpeed = moveSpeed;
        }

        Vector3 movement = new Vector3(moveHoriz, 0, moveVertic) * curSpeed;
        movement = transform.rotation * movement;

        transform.position += movement;
    }
}
