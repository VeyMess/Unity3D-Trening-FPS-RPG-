using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWayPoint : MonoBehaviour
{
    public Transform nextDestination;
    public float pauseToGo = 0f;

    private GameObject Ai;


    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag.Equals("Enemy"))
        {
            if (coll.attachedRigidbody != null)
            {
                EnemyMove tempMove = coll.gameObject.GetComponentInParent<EnemyMove>();

                //If crossed trigger Equals AI destination, then change next destination, after pause
                if (tempMove.CheckDestin(transform))
                {
                    tempMove.SetDestin(nextDestination, pauseToGo);
                }
            }
        }
    }
}
