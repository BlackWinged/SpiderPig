using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testGenerate : MonoBehaviour
{
    public Transform square;
    public BoxCollider2D upperLimit;
    public BoxCollider2D lowerLimit;
    public Transform startPoint;
    public float maxDelta;
    public float minDistance;
    public int segmentBlockNumber;
    public float chanceOfEnemy = 0.10f;
    public List<Transform> potentialEnemies;

    private float upperLimitY;
    private float lowerLimitY;
    private float startPointX;

    private Transform previousPositionUpper = null;
    private Transform previousPositionLower = null;

    public Transform currentPositionUpper = null;
    private Transform currentPositionLower = null;
    // Use this for initialization
    void Start()
    {
        upperLimitY = upperLimit.transform.position.y - upperLimit.bounds.extents.y;
        lowerLimitY = lowerLimit.transform.position.y + lowerLimit.bounds.extents.y;
        Destroy(upperLimit.gameObject);
        Destroy(lowerLimit.gameObject);
        startPointX = startPoint.position.x;
        for (int i = 0; i < segmentBlockNumber; i++)
        {
            setSegment();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setSegment()
    {
        int segmentIteration = 0;
        do
        {
            createSegmentUpper();
            createSegmentDowner();

            float squareExtent = square.GetComponent<BoxCollider2D>().bounds.extents.y;
            float currPosYUpper = currentPositionUpper.transform.position.y - squareExtent;
            float currPosYLower = currentPositionLower.transform.position.y + squareExtent;

            if (currPosYUpper - currPosYLower < minDistance)
            {
                resetLevelSegment();
            }

            if (previousPositionUpper != null)
            {
                float prevPosYUpper = previousPositionUpper.transform.position.y - squareExtent;
                float prevPosYLower = previousPositionLower.transform.position.y + squareExtent;

                if (Mathf.Abs(currPosYUpper - prevPosYLower) < minDistance)
                {
                    resetLevelSegment();
                }

                if (Mathf.Abs(currPosYLower - prevPosYUpper) < minDistance)
                {
                    resetLevelSegment();
                }
            }
            segmentIteration++;
            if (segmentIteration > 100)
            {
                Debug.Log("Infinite loop while generating level; breaking...");
                break;
            }
        } while (currentPositionUpper == null);
        checkForEnemy();
    }

    void createSegmentUpper()
    {
        if (currentPositionUpper != null)
        {
            previousPositionUpper = currentPositionUpper;
        }
        float squareWidth = square.GetComponent<BoxCollider2D>().bounds.size.x;
        //because stupid pivot points -.-
        float yPosition = upperLimitY - square.GetComponent<BoxCollider2D>().bounds.extents.y;
        //done

        if (previousPositionUpper == null)
        {
            currentPositionUpper = (Transform)Instantiate(square, new Vector3(startPointX, yPosition + (maxDelta * Random.value), 0), new Quaternion(0, 0, 0, 0));
        }
        else
        {
            currentPositionUpper = (Transform)Instantiate(square, new Vector3(previousPositionUpper.transform.position.x + squareWidth, yPosition + (maxDelta * Random.value), 0), new Quaternion(0, 0, 0, 0));
        }
    }

    void createSegmentDowner()
    {
        if (currentPositionLower != null)
        {
            previousPositionLower = currentPositionLower;
        }
        float squareWidth = square.GetComponent<BoxCollider2D>().bounds.size.x;

        //because stupid pivot points -.-
        float yPosition = lowerLimitY - square.GetComponent<BoxCollider2D>().bounds.extents.y;
        //done

        if (previousPositionLower == null)
        {
            currentPositionLower = (Transform)Instantiate(square, new Vector3(startPointX, yPosition - (maxDelta * Random.value), 0), new Quaternion(0, 0, 0, 0));
        }
        else
        {
            currentPositionLower = (Transform)Instantiate(square, new Vector3(previousPositionLower.transform.position.x + squareWidth, yPosition - (maxDelta * Random.value), 0), new Quaternion(0, 0, 0, 0));
        }
    }

    void resetLevelSegment()
    {
        if (currentPositionUpper != null && currentPositionLower != null)
        {
            Destroy(currentPositionUpper.gameObject);
            Destroy(currentPositionLower.gameObject);
            currentPositionUpper = null;
            currentPositionLower = null;
        }
    }

    void checkForEnemy()
    {
        int pickEnemy = (int)Mathf.Floor(potentialEnemies.Count * Random.value);
        if (Random.value <= chanceOfEnemy && potentialEnemies.Count>0)
        {
            float squareExtent = square.GetComponent<BoxCollider2D>().bounds.extents.y;
            float currPosYLower = currentPositionLower.transform.position.y + squareExtent*2;
            float currPosYUpper = currentPositionUpper.transform.position.y;
            float y = (currPosYUpper - currPosYLower) * Random.value;
            y += currPosYLower;
            Vector3 position = new Vector3(currentPositionUpper.position.x, y);

            Instantiate(potentialEnemies[pickEnemy], position, new Quaternion(0,0,0,0));

        }
    }
}
