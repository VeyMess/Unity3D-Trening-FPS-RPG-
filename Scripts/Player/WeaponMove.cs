using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMove : MonoBehaviour {

    public Animator playerAnim;
    public Camera mainCam;
    public Transform posForCam;
    public Transform weaponPos;

    private PlayerMovement playMove;
    private float MemSens;
    private Vector3 startPos;
    private float check;
    private bool rot;
    private bool sigh;
    private Vector3 toPoint;



    void Start()
    {
        playMove = GetComponentInParent<PlayerMovement>();
        toPoint = new Vector3(weaponPos.localPosition.x + .11f, weaponPos.localPosition.y + .05f, weaponPos.localPosition.z + .11f);
        startPos = weaponPos.localPosition;
        check = 0;
        rot = false;
        sigh = false;
    }

    void Update()
    {
        if (!GetComponentInParent<UIControlling>().GetInInventory())
        {
            //if Walk Animation....
            if (playerAnim.GetBool("Move") && !playerAnim.GetBool("Run") && sigh == false)
            {
                if (rot == false)
                {
                    weaponPos.localPosition = Vector3.Slerp(weaponPos.localPosition, toPoint, 2 * Time.deltaTime);
                    check += Time.deltaTime;
                    if (check >= .8f)
                        rot = true;
                }
                else
                {
                    weaponPos.localPosition = Vector3.Slerp(weaponPos.localPosition, startPos, 2.2f * Time.deltaTime);
                    check -= Time.deltaTime;
                    if (check <= 0f)
                        rot = false;
                }
            }
            //... or run animation ...
            else if (playerAnim.GetBool("Move") && playerAnim.GetBool("Run") && sigh == false)
            {
                if (rot == false)
                {
                    weaponPos.localPosition = Vector3.Slerp(weaponPos.localPosition, toPoint, 4 * Time.deltaTime);
                    check += Time.deltaTime;
                    if (check >= .6f)
                        rot = true;
                }
                else
                {
                    weaponPos.localPosition = Vector3.Slerp(weaponPos.localPosition, startPos, 2.2f * Time.deltaTime);
                    check -= Time.deltaTime;
                    if (check <= 0f)
                        rot = false;
                }
            }
            //... or return gun to normal position
            else if (sigh == false)
                weaponPos.localPosition = Vector3.Slerp(weaponPos.localPosition, startPos, 10 * Time.deltaTime);
            if (GetComponent<WeaponProp>().type == WeaponProp.WeaponType.M4A1)
            {
                Sight();
            }
        }
    }

    //transform gun for looking troe sight
    void Sight()
    {
        if (Input.GetButton("Fire2"))
        {
            weaponPos.localPosition = Vector3.Slerp(weaponPos.localPosition, posForCam.localPosition, 20 * Time.deltaTime);            
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView,3,10*Time.deltaTime);
            if(!sigh)
            {
                MemSens = playMove.sensensity;
                playMove.sensensity *= playMove.ratioForSensInZoom;
            }
            sigh = true;
        }
        else if(!Input.GetButton("Fire2") && sigh)
        {
            weaponPos.localPosition = Vector3.Slerp(weaponPos.localPosition, startPos, 30 * Time.deltaTime);
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, 60, 10 * Time.deltaTime);
            if (weaponPos.localPosition == startPos)
            {
                sigh = false;
                playMove.sensensity = MemSens;
            }
        }
    }

    public bool InSight()
    {
        return sigh;
    }
}
