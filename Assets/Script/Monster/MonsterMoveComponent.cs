using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterMoveComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 moveDir;
    public Vector3 returnPos;
    public Vector3 returnMoveDir;
    public int roadNum;
    public Status status;

    public bool isRoad = true;
    void Start()
    {
        returnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRoad)
        {
            transform.position += moveDir * status.MoveSpeed * Time.deltaTime;
            returnMoveDir = Vector3.zero;
        }
        else
        {
            if ((int)returnMoveDir.x == 0 && (int)returnMoveDir.y == 0)
                returnMoveDir = FindCloseRoadDir();

            transform.position += returnMoveDir * status.MoveSpeed * Time.deltaTime;
        }



        if (Mathf.Abs(transform.position.x - returnPos.x) > 0.5f && (roadNum == 1 || roadNum == 3))
            isRoad = false;
        else if (Mathf.Abs(transform.position.x - returnPos.x) <= 0.1f && (roadNum == 1 || roadNum == 3))
            isRoad = true;

        if (Mathf.Abs(transform.position.y - returnPos.y) > 0.5f && (roadNum == 2 || roadNum == 4))
            isRoad = false;
        else if (Mathf.Abs(transform.position.y - returnPos.y) <= 0.1f && (roadNum == 2 || roadNum == 4))
            isRoad = true;

    }

    Vector2 FindCloseRoadDir()
    {
        List<float> distances = new List<float>();

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    distances.Add(RoadDistance(Vector2.left));
                    break;
                case 1:
                    distances.Add(RoadDistance(Vector2.right));
                    break;
                case 2:
                    distances.Add(RoadDistance(Vector2.up));
                    break;
                case 3:
                    distances.Add(RoadDistance(Vector2.down));
                    break;
            }
        }

        float minDistance = distances.Min();
        int minIndex = distances.IndexOf(minDistance);

        if (minIndex == 0)
            return Vector2.left;
        else if (minIndex == 1)
            return Vector2.right;
        else if (minIndex == 2)
            return Vector2.up;
        else if (minIndex == 3)
            return Vector2.down;

        return Vector2.zero;
    }

    float RoadDistance(Vector2 dir)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir,100);
        float distance = 10000;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Road"))
            {
                distance = hit.distance;
                break;
            }
        }
        return distance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Road") && !isRoad)
        {
            moveDir = collision.GetComponent<RoadComponent>().moveDir;
            roadNum = collision.GetComponent<RoadComponent>().roadNum;
            returnPos = collision.GetComponent<RoadComponent>().returnPos;
        }
    }
}

