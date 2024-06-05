using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLoop : MonoBehaviour
{

    [SerializeField] private List<Transform> roadSegments;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float resetPositionZ = -10f;
    [SerializeField] private float roadSegmentLength = 10f;  //���� ���� ����


    private void Update()
    {
        RepeatRoad();
    }

    private void RepeatRoad()
    {
        for(int i= 0; i<roadSegments.Count; i++)
        {
            roadSegments[i].Translate(Vector3.back * speed * Time.deltaTime);   //���� z�� �̵�

            //resetPositionZ ���� �� �ڷ� �̵�
            if(roadSegments[i].position.z <= resetPositionZ)
            {
                Vector3 newPos = roadSegments[GetLastIndex()].position;
                newPos.z += roadSegmentLength;

                //������ ���� �� �ڷ� �̵�
                roadSegments[i].position = newPos;

                Transform temp = roadSegments[i];
                roadSegments.RemoveAt(i);
                roadSegments.Add(temp);     //List���� ���� �� �� �ڿ� �߰�-> ���� ����

                i--; 
            }
        }
    }

    private int GetLastIndex()
    {
        return roadSegments.Count - 1;  //������ ������ �ε��� ��ȯ
    }
}
