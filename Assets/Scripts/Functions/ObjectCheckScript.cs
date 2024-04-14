using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCheckScript : MonoBehaviour
{
    public int numberOfSpawnEnemyTriggers;
    public int numberOfKnightSkeletons;
    public GameObject Portal;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        numberOfSpawnEnemyTriggers = GameObject.FindGameObjectsWithTag("SpawnEnemyTrigger").Length;
        numberOfKnightSkeletons = GameObject.FindGameObjectsWithTag("knightSkeleton").Length;

        if(numberOfSpawnEnemyTriggers == 0 && numberOfKnightSkeletons == 0) { 
            Portal.SetActive(true);
        }
    }
}
