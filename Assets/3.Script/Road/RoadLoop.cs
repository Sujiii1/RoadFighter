using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoadLoop : MonoBehaviour
{
    [SerializeField] private TMP_Text speedText;

    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float maxSpeed = 50.0f;
    [SerializeField] private float accelerationspeed = 1f;


    //Road Repeat
    [SerializeField] private List<Transform> roadSegments;
    [SerializeField] private float resetPositionZ = -10f;
    [SerializeField] private float roadSegmentLength = 10f;  //���� ���� ����

    private void Start()
    {
        speed = 0f;
        ScoreManager.Instance.isStartGame = false;
    }

    private void Update()
    {
        
        if(ScoreManager.Instance.isStartGame)
        {
            RepeatRoad();
            UpdateSpeedText();
        }
    }

    private void RepeatRoad()
    {
        ScoreManager.Instance.isStartGame = true;
        for (int i = 0; i < roadSegments.Count; i++)
        {
            if (speed < maxSpeed)
            {
                speed += accelerationspeed * Time.deltaTime;
            }
            roadSegments[i].Translate(Vector3.back * speed * Time.deltaTime);   //���� z�� �̵�


            //resetPositionZ ���� �� �ڷ� �̵�
            if (roadSegments[i].position.z <= resetPositionZ)
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

    private void UpdateSpeedText()      //Speed UI
    {
        float displayedSpeed = Mathf.Clamp(speed, 0f, maxSpeed); // �ӵ��� �ִ밪(maxSpeed)���� Ŭ����
        float normalizedSpeed = (displayedSpeed / maxSpeed) * 400f; 
        speedText.text = Mathf.RoundToInt(normalizedSpeed) + "  Km/h"; 
    }


    public float GetSpeed()
    {
        return speed;
    }

    public void ZeroSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = Mathf.Clamp(newSpeed, 0f, maxSpeed); // ���ο� �ӵ��� Ŭ����
    }


}
