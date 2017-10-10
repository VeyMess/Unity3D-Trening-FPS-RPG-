using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {

    public GameObject doorAnimation;
    public GameObject mainTerr;
    public GameObject[] spawns;

void OnTriggerEnter(Collider coll)
    {
        if(coll.tag.Equals("Player"))
        {
            doorAnimation.GetComponent<Animator>().SetTrigger("Open");
            mainTerr.SetActive(true);
            foreach (GameObject temp in spawns)
            {
                temp.SetActive(true);
            }
        }
    }
}
