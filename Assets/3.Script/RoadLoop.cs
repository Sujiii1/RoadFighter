using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLoop : MonoBehaviour
{

    [SerializeField] private List<Transform> roadSegments;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float resetPositionZ = -10f;
    [SerializeField] private float roadSegmentLength = 10f;  //도로 조각 길이


    private void Update()
    {
        RepeatRoad();
    }

    private void RepeatRoad()
    {
        for(int i= 0; i<roadSegments.Count; i++)
        {
            roadSegments[i].Translate(Vector3.back * speed * Time.deltaTime);   //도로 z축 이동

            //resetPositionZ 에서 맨 뒤로 이동
            if(roadSegments[i].position.z <= resetPositionZ)
            {
                Vector3 newPos = roadSegments[GetLastIndex()].position;
                newPos.z += roadSegmentLength;

                //마지막 도로 맨 뒤로 이동
                roadSegments[i].position = newPos;

                Transform temp = roadSegments[i];
                roadSegments.RemoveAt(i);
                roadSegments.Add(temp);     //List에서 제거 후 맨 뒤에 추가-> 순서 유지

                i--; 
            }
        }
    }

    private int GetLastIndex()
    {
        return roadSegments.Count - 1;  //마지막 도로의 인덱스 반환
    }
}
