using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawn : MonoBehaviour
{
    [SerializeField] private GameObject roadPrefebs;
    private Vector3 reRoadPos = new Vector3(0,0,10);

    private void Update()
    {
        createRoad();
    }

    private void createRoad()
    {
        Instantiate(roadPrefebs, reRoadPos, roadPrefebs.transform.rotation);
    }
}
