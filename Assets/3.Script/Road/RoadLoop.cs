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
    [SerializeField] private float roadSegmentLength = 10f;  //도로 조각 길이

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
            roadSegments[i].Translate(Vector3.back * speed * Time.deltaTime);   //도로 z축 이동


            //resetPositionZ 에서 맨 뒤로 이동
            if (roadSegments[i].position.z <= resetPositionZ)
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

    private void UpdateSpeedText()      //Speed UI
    {
        float displayedSpeed = Mathf.Clamp(speed, 0f, maxSpeed); // 속도를 최대값(maxSpeed)까지 클램핑
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
        speed = Mathf.Clamp(newSpeed, 0f, maxSpeed); // 새로운 속도를 클램핑
    }


}
