using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterMoveComponent : MonoBehaviour
{
    public Vector2 mapTopLeft = new Vector2(-2f, 2f); // 맵의 11시 좌표
    public Vector2 mapBottomLeft = new Vector2(-2f, -2f); // 맵의 7시 좌표
    public Vector2 mapBottomRight = new Vector2(2f, -2f); // 맵의 5시 좌표
    public Vector2 mapTopRight = new Vector2(2f, 2f); // 맵의 1시 좌표
    public Status status;

    private Vector2[] pathPoints; // 경로 좌표
    private int currentSegment = 0; // 현재 이동 중인 구간

    void Start()
    {
        // 경로 설정
        pathPoints = new Vector2[]
        {
            mapTopLeft,     // 11시
            mapBottomLeft,  // 7시
            mapBottomRight, // 5시
            mapTopRight     // 1시
        };
    }

    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        // 현재 구간의 시작점과 끝점
        Vector2 start = pathPoints[currentSegment];
        Vector2 end = pathPoints[(currentSegment + 1) % pathPoints.Length];

        // 일정한 속도로 이동
        transform.position = Vector2.MoveTowards(transform.position, end, status.MoveSpeed * Time.deltaTime);

        // 도착했는지 확인
        if (Vector2.Distance(transform.position, end) < 0.1f)
        {
            currentSegment = (currentSegment + 1) % pathPoints.Length; // 다음 구간으로
        }
    }
    public void ResetCurrentSegment()
    {
        currentSegment = 0;
    }
}

