using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerControl : MonoBehaviour
{
    public LayerMask collideables;
    public Transform rope;
    public float maxRopeLength;
    public Vector2 reactionForce;
    public float reactionTorque;
    public float expansionTime = 0.5f;
    public float expansionPercentage = 0.10f;
    public float expansionSmoothing = 5f;
    public float centriPetalForceModifier = 100f;
    public float backupWidth = 10f;

    private bool inputDown = false;
    private Dictionary<int, Transform> ropes = new Dictionary<int, Transform>();
    private Dictionary<int, Vector3> hitpoints = new Dictionary<int, Vector3>();
    private Dictionary<int, DistanceJoint2D> joints = new Dictionary<int, DistanceJoint2D>();
    private Dictionary<int, int> ropeStatus = new Dictionary<int, int>();
    private Dictionary<int, float> expansionTimers = new Dictionary<int, float>();
    private Dictionary<int, float> distanceBuffers = new Dictionary<int, float>();

    private float ropeSegmentLength;

    private const int ROPE_FIRED = 0;
    private const int ROPE_EXPANDING = 1;
    private const int ROPE_SET = 2;

    // Use this for initialization
    void Start()
    {
        Transform trans = ((Transform)Instantiate(rope, new Vector3(0, 0, -50), new Quaternion(0, 0, 0, 0)));
        ropeSegmentLength = trans.GetComponent<MeshRenderer>().bounds.size.x;
        Destroy(trans.gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        extendRope();
        if (GetComponent<playerInput>().touch)
        {
            if (!inputDown)
            {
                inputDown = true;
                createRope();
                extendRope();
            }
            else
            {
            }
        }
        if (inputDown)
        {
            if (!GetComponent<playerInput>().touch)
            {
                ropeDestruction();
            }
        }
    }

    void extendRope()
    {
        foreach (int key in ropeStatus.Keys.ToList())
        {
            int status;
            ropeStatus.TryGetValue(key, out status);

            if (status == ROPE_EXPANDING || status == ROPE_SET)
            {

                Vector3 anchorPoint;
                Transform rope;
                DistanceJoint2D jointInstance;
                float expansionTimerInstance;
                float distanceInstance;


                ropes.TryGetValue(key, out rope);
                joints.TryGetValue(key, out jointInstance);
                expansionTimers.TryGetValue(key, out expansionTimerInstance);

                if (jointInstance.connectedBody == null)
                {
                    hitpoints.TryGetValue(key, out anchorPoint);
                }
                else
                {
                    anchorPoint = jointInstance.connectedBody.transform.TransformPoint(jointInstance.connectedAnchor);
                }


                if (!distanceBuffers.TryGetValue(key, out distanceInstance))
                {
                    distanceInstance = jointInstance.distance;
                    distanceBuffers.Add(key, distanceInstance);
                }

                jointInstance.distance = Vector2.Distance(anchorPoint, transform.position);

                if (distanceInstance > jointInstance.distance + 0.01)
                {
                    status = ROPE_SET;
                }
                if (status == ROPE_EXPANDING)
                {
                    float timeRatio;
                    expansionTimers.TryGetValue(key, out timeRatio);
                    timeRatio /= expansionTime;
                    if (timeRatio == 1)
                    {
                        timeRatio = 0.99999f;
                    }
                    jointInstance.distance = Mathf.Lerp(jointInstance.distance, distanceInstance * (1 + expansionPercentage), expansionSmoothing * Time.deltaTime);
                    expansionTimerInstance -= Time.deltaTime;
                    expansionTimers.Remove(key);
                    expansionTimers.Add(key, expansionTimerInstance);
                    if (expansionTimerInstance <= 0)
                    {
                        status = ROPE_SET;
                        ropeStatus[key] = status;
                    }
                }

                if (GetComponent<playerInput>().scroll > 0)
                {
                    //jointInstance.distance -= GetComponent<playerInput>().scroll * GetComponent<playerInput>().scrollCoef * Time.deltaTime;
                    Vector2 force = anchorPoint - transform.position;
                    force = force.normalized;
                    force *= centriPetalForceModifier;
                    if (jointInstance.connectedBody != null)
                    {
                        GetComponent<Rigidbody2D>().AddForce(force);
                        jointInstance.connectedBody.AddForceAtPosition(force * -1, anchorPoint);
                    }
                    else
                    {
                        GetComponent<Rigidbody2D>().AddForce(force);
                    }
                }

                drawBetweenPoints(transform.position, anchorPoint, rope.gameObject);
            }
        }
    }

    public void extendRope(bool addSpeed)
    {
        foreach (int key in ropeStatus.Keys.ToList())
        {
            int status;
            ropeStatus.TryGetValue(key, out status);

            if (status == ROPE_EXPANDING || status == ROPE_SET)
            {

                Vector3 anchorPoint;
                Transform rope;
                DistanceJoint2D jointInstance;
                float expansionTimerInstance;
                float distanceInstance;


                ropes.TryGetValue(key, out rope);
                joints.TryGetValue(key, out jointInstance);
                expansionTimers.TryGetValue(key, out expansionTimerInstance);

                if (jointInstance.connectedBody == null)
                {
                    hitpoints.TryGetValue(key, out anchorPoint);
                }
                else
                {
                    anchorPoint = jointInstance.connectedBody.transform.TransformPoint(jointInstance.connectedAnchor);
                }


                if (!distanceBuffers.TryGetValue(key, out distanceInstance))
                {
                    distanceInstance = jointInstance.distance;
                    distanceBuffers.Add(key, distanceInstance);
                }

                jointInstance.distance = Vector2.Distance(anchorPoint, transform.position);

                if (distanceInstance > jointInstance.distance + 0.01)
                {
                    status = ROPE_SET;
                }
                if (status == ROPE_EXPANDING)
                {
                    float timeRatio;
                    expansionTimers.TryGetValue(key, out timeRatio);
                    timeRatio /= expansionTime;
                    if (timeRatio == 1)
                    {
                        timeRatio = 0.99999f;
                    }
                    jointInstance.distance = Mathf.Lerp(jointInstance.distance, distanceInstance * (1 + expansionPercentage), expansionSmoothing * Time.deltaTime);
                    expansionTimerInstance -= Time.deltaTime;
                    expansionTimers.Remove(key);
                    expansionTimers.Add(key, expansionTimerInstance);
                    if (expansionTimerInstance <= 0)
                    {
                        status = ROPE_SET;
                        ropeStatus[key] = status;
                    }
                }

                if (GetComponent<playerInput>().scroll > 0 && addSpeed)
                {
                    Vector2 force = anchorPoint - transform.position;
                    force = force.normalized;
                    force *= centriPetalForceModifier;
                    if (jointInstance.connectedBody != null)
                    {
                        GetComponent<Rigidbody2D>().AddForce(force );
                        jointInstance.connectedBody.AddForceAtPosition(force * -1, jointInstance.connectedBody.transform.InverseTransformPoint(anchorPoint));
                    }
                    else
                    {
                        GetComponent<Rigidbody2D>().AddForce(force);
                    }
                }

                drawBetweenPoints(transform.position, anchorPoint, rope.gameObject);
            }
        }
    }

    void createRope()
    {
        //for (int = 0; i<=)
        Vector3 direction = GetComponent<playerInput>().positions[0] - transform.position;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction, maxRopeLength, collideables);
        if (hit.collider == null)
        {
            float angle = Quaternion.LookRotation(Vector3.forward, direction).eulerAngles.z;
            hit = Physics2D.BoxCast(transform.position + direction.normalized * 10, new Vector2(backupWidth, backupWidth), angle, direction, maxRopeLength, collideables);
            if (GetComponent<CircleCollider2D>().IsTouching(hit.collider))
            {
                return;
            }
        }


        if (hit.collider != null)
        {
            Transform trans = ((Transform)Instantiate(rope, transform.position, rightfaceRotate(direction)));
            trans.SetParent(transform);

            DistanceJoint2D distJoint = gameObject.AddComponent<DistanceJoint2D>();
            Vector3 anchorPosition = hit.point;
            distJoint.distance = Vector2.Distance(anchorPosition, transform.position);
            //distJoint.distance = 999;
            distJoint.connectedAnchor = anchorPosition;
            distJoint.enableCollision = true;
            distJoint.maxDistanceOnly = true;
            if (hit.collider.transform.tag.Equals(Tags.ENEMY))
            {
                distJoint.connectedAnchor = hit.collider.transform.InverseTransformPoint(anchorPosition);
                distJoint.connectedBody = hit.collider.GetComponent<Rigidbody2D>();
            }

            joints.Add(1, distJoint);
            ropes.Add(1, trans);
            hitpoints.Add(1, hit.point);
            ropeStatus.Add(1, ROPE_EXPANDING);
            expansionTimers.Add(1, expansionTime);
        }
    }


    Quaternion rightfaceRotate(Vector3 direction)
    {
        return Quaternion.LookRotation(Vector3.forward, direction) * Quaternion.Euler(0, 0, 90);
    }

    void drawBetweenPoints(Vector3 from, Vector3 to, GameObject drawSubject)
    {
        Vector3 halfwayPoint = Vector3.Lerp(from, to, 0.5f);
        drawSubject.transform.position = halfwayPoint;
        float scaleCoef = Vector3.Distance(from, to) / ropeSegmentLength;
        Vector3 scaledSize = drawSubject.transform.localScale;
        scaledSize.x = scaleCoef;
        drawSubject.transform.localScale = scaledSize;
        Vector3 direction = from - to;
        drawSubject.transform.rotation = rightfaceRotate(direction);

    }

   public void ropeDestruction()
    {
        inputDown = false;
        DistanceJoint2D jointInstance;
        Transform ropeInstance;
        foreach (int key in ropeStatus.Keys)
        {
            joints.TryGetValue(1, out jointInstance);
            ropes.TryGetValue(1, out ropeInstance);
            Destroy(jointInstance);
            Destroy(ropeInstance.gameObject);
            expansionTimers.Clear();
            distanceBuffers.Clear();
            joints.Remove(1);
            ropes.Remove(1);
            hitpoints.Remove(1);
        }
        ropeStatus.Clear();
    }

}
