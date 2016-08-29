using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGeneration : MonoBehaviour
{

    public Transform enemy;
    public Transform platform;
    public int DistanceBetweenPlatforms = 2;
    public int numberOfPlatforms = 200;

    public Transform spawnSegment;
    public List<Transform> allSegments;
    public int segmentNumber;
    public float EnemyPlatformPercentage = 10;

    private float segmentLength;

    private List<Transform> platforms = new List<Transform>();
    private int numberOfOffShoots;
    private List<Transform> upperFloor = new List<Transform>();
    private List<Transform> downFloor = new List<Transform>();

    Vector2 previousStartPoint = new Vector2();
    Vector2 previousEndPoint = new Vector2();
    Vector2 previousPlatformPoint = new Vector2();

    // Use this for initialization
    void Start()
    {
        List<Transform> points = new List<Transform>();
        points.AddRange(spawnSegment.GetComponentsInChildren<Transform>());
        findSegmentPoints(points, out previousStartPoint, out previousEndPoint, out previousPlatformPoint);

        segmentPlacement();
        enemyPlacement();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
    }

    void segmentPlacement()
    {
        for (int i = 0; i < segmentNumber; i++)
        {
            List<Transform> availableSegments = spawnSegment.GetComponent<SegmentFactory>().getAvailableSegments();
            int pickSegment = (int)(availableSegments.Count * Random.value);
            if (pickSegment == availableSegments.Count) pickSegment--;
            //Transform segment = Instantiate(availableSegments[pickSegment]);
            Transform segment = null;
            foreach (Transform singleSegment in allSegments)
            {
                if (availableSegments[pickSegment].name.StartsWith(singleSegment.name))
                {
                    segment = Instantiate(singleSegment);
                    platforms.Add(segment);
                }
            }

            List<Transform> points = new List<Transform>();
            points.AddRange(segment.GetComponentsInChildren<Transform>());
            Vector2 startPoint = new Vector2();
            Vector2 endPoint = new Vector2();
            Vector2 platformPoint = new Vector2();
            findSegmentPoints(points, out startPoint, out endPoint, out platformPoint);
            Vector2 positionOffset = (Vector2)segment.position - startPoint;
            segment.position = previousEndPoint + positionOffset;
            findSegmentPoints(points, out startPoint, out endPoint, out platformPoint);
            platformGeneration(previousPlatformPoint, platformPoint, 0);
            previousStartPoint = startPoint;
            previousEndPoint = endPoint;
            previousPlatformPoint = platformPoint;
            spawnSegment = segment;
        }
    }

    void findSegmentPoints(List<Transform> points, out Vector2 startPoint, out Vector2 endPoint, out Vector2 platformPoint)
    {
        startPoint = new Vector2();
        endPoint = new Vector2();
        platformPoint = new Vector2();
        foreach (Transform point in points)
        {
            if (point.tag.Equals(Tags.SEGMENT_START_POINT))
            {
                startPoint = point.position;
            }
            if (point.tag.Equals(Tags.SEGMENT_END_POINT))
            {
                endPoint = point.position;
            }
            if (point.tag.Equals(Tags.SEGMENT_PLATFORM_POINT))
            {
                platformPoint = point.position;
            }
        }
    }

    void platformGeneration(Vector2 center, Vector2 radiusPoint, int level)
    {
        Vector2 position = new Vector2(0, 0);
        int i = 0, killCount = 0;
        while (i < numberOfPlatforms && killCount < 10)
        {
            position = Random.insideUnitCircle;
            position *= Vector2.Distance(center, radiusPoint);
            position += center;

            bool isTooClose = false;
            foreach (Transform platform in platforms)
            {
                if (Vector2.Distance(platform.position, position) < DistanceBetweenPlatforms)
                {
                    isTooClose = true;
                    break;
                }
            }
            if (!isTooClose)
            {
                platforms.Add((Transform)GameObject.Instantiate(platform, position, new Quaternion(0, 0, 0, 0)));
                i++;
                killCount = 0;
            } else
            {
                killCount++;
            }

        }
    }

    private void enemyPlacement()
    {
        foreach (Transform platform in platforms)
        {
            if (Random.value*100 <= EnemyPlatformPercentage)
            {
                Instantiate(enemy, (Vector2) platform.position + new Vector2(0, 5), new Quaternion(0, 0, 0, 0));
            }
        }
    }



}
