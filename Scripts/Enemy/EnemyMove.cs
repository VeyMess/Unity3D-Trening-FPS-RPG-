using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    private NavMeshAgent enemNav;
    private Animator anim;
    private Transform nextDestin;

    private bool GoodToGo = true;
    private float waitTime = 0f;
    private float pass = 0f;

    public bool isAlive = true;
    private bool disabled = false;

    private bool isAgro = false;
    public float timeBetwenAtacks = 1f;
    private float timerAtc = 0f;
    private bool isAtacking = false;
    private bool inRange = false;


    public float walkSpeed = 1f;
    public float runSpeed = 10f;
    public bool hasRoot = false;
    private Transform agroTarget;

    public Transform startDestination;



    void Awake()
    {
        enemNav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if(startDestination != null)
        {
            nextDestin = startDestination;
            hasRoot = true;
        }
    }

    void Start()
    {
        float minDist = 999999;
        foreach (AIWayPoint temp in FindObjectsOfType<AIWayPoint>())
        {
            float dist = Vector3.Distance(transform.position, temp.transform.position);
            if(minDist > dist)
            {
                minDist = dist;
                nextDestin = temp.transform;
            }
        }
        if (minDist != 999999)
            hasRoot = true;
    }

    void Update()
    {
        if (isAlive)
        {
            if (hasRoot)
            {
                CheckDist();
                if (!isAgro)
                {
                    enemNav.speed = walkSpeed;
                    if (GoodToGo && waitTime == 0f)
                    {
                        if (nextDestin == null)
                        { }
                        else
                            enemNav.SetDestination(nextDestin.position);
                    }

                    //if (enemNav.destination != transform.position)
                    if (Vector3.Distance(enemNav.destination, transform.position) > 1) 
                    {
                        anim.SetBool("Move", true);
                    }
                    else
                    {
                        anim.SetBool("Move", false);
                        GoodToGo = false;

                        if (pass < waitTime)
                        {
                            pass += Time.deltaTime;
                        }
                        else
                        {
                            pass = 0f;
                            GoodToGo = true;
                            waitTime = 0f;
                        }
                    }
                }
                else
                {
                    anim.SetBool("Run", true);
                    anim.SetBool("Move", true);
                    enemNav.SetDestination(agroTarget.position);
                    enemNav.speed = runSpeed;
                }
            }
        }
        else
        {
            enemNav.enabled = false;
            anim.SetTrigger("Die");
            GetComponent<Rigidbody>().Sleep();
            foreach (Collider temp in GetComponentsInChildren<Collider>())
            {
                temp.enabled = false;
            }
            Destroy(gameObject, 5f);
            GetComponent<Rigidbody>().WakeUp();
        }
    }


    public void SetDestin(Transform nextPos, float sleepGo)
    {
        nextDestin = nextPos;
        waitTime = sleepGo;
    }

    // Check what waypoint crossed AI (Desired or Not)
    public bool CheckDestin(Transform coll)
    {
        if (hasRoot)
            return nextDestin.Equals(coll);
        else
            return true;
    }

    public void SetAgro(Transform target)
    {
        isAgro = true;
        agroTarget = target;
    }

    void CheckDist()
    {
        PlayerMovement[] temp = FindObjectsOfType<PlayerMovement>();
        foreach(PlayerMovement ins in temp)
        {

            if(Vector3.Distance(ins.transform.position,transform.position) < 15)
            {
                SetAgro(ins.transform);
            }
            if(Vector3.Distance(ins.transform.position,transform.position) < GetComponent<EnemyStats>().EnemyAttackRange)
            {
                anim.SetBool("InRange", true);
                isAtacking = true;
                inRange = true;
            }
            else
            {
                anim.SetBool("InRange", false);
                inRange = false;
            }
            if (isAtacking)
            {
                timerAtc += Time.deltaTime;
                if (timerAtc >= timeBetwenAtacks && inRange)
                {
                    ins.GetComponent<PlayerHealth>().HealthChange(-GetComponent<EnemyStats>().enemyDamage);
                    timerAtc = 0;
                    isAtacking = false;
                }
                else if(timerAtc >= timeBetwenAtacks && !inRange)
                {
                    timerAtc = 0;
                    isAtacking = false;
                }
            }

        }
    }

    public void AnimHit()
    {
        if(!anim.GetBool("PlayingHit"))
            anim.SetTrigger("Hit");
    }

}
