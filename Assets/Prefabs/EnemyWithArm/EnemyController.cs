using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float maxHeight;
    public float maxEyeSize;
    public float maxSpeed;

    public GameObject eye;
    public Transform arm;
    public Transform armTip;
    public Transform armTipPosition;
    public float maxArmLength;

    public float armWindupTimer = 2;
    public float armFireTimer = 0.1f;
    public float coolDownTimer = 2;

    public float killSpeedTreshold = 20;
    public float enemyActivateDistance = 40;

    private float armWindupTimerBuffer = 2;
    private float armFireTimerBuffer = 0;
    private float coolDownTimerBuffer = 0;


    private Transform player;
    private CircleCollider2D playerCollider;
    private float armLength;
    private int frameCount = 5;
    // Use this for initialization
    void Start()
    {
        arm = ((Transform)Instantiate(arm, new Vector3(0, 0, -50), new Quaternion(0, 0, 0, 0)));
        arm.parent = transform.parent;
        armLength = arm.GetComponent<MeshRenderer>().bounds.size.x;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerCollider == null)
        {
            playerCollider = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponentInChildren<CircleCollider2D>();
            player = playerCollider.transform;
            createFoe();
        }
        CollisionHandling();
        armWave();
    }

    private void armWave()
    {
        if (frameCount > 0 && Vector2.Distance(transform.position, player.transform.position) <= enemyActivateDistance)
        {
            armTip.GetComponent<DistanceJoint2D>().distance = maxArmLength;
            armTip.GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized * 20000);
            frameCount--;
        }
        if (coolDownTimerBuffer > 0 && frameCount == 0)
        {
            coolDownTimerBuffer -= Time.deltaTime;
            if (coolDownTimerBuffer <= 0)
            {
                armWindupTimerBuffer = armWindupTimer;
            }
        }
        if (frameCount == 0 && armWindupTimerBuffer > 0)
        {
            float reductionFactor = Time.deltaTime / armWindupTimer;
            reductionFactor = maxArmLength * reductionFactor;
            armTip.GetComponent<DistanceJoint2D>().distance -= reductionFactor;
            armWindupTimerBuffer -= Time.deltaTime;
            if (armWindupTimerBuffer <= 0)
            {
                armWindupTimerBuffer = 0;
                frameCount = 5;
                coolDownTimerBuffer = coolDownTimer;
            }
        }
        drawBetweenPoints(transform.position, armTip.position, arm.gameObject);
    }

    private void CollisionHandling()
    {
        if (eye.GetComponent<BoxCollider2D>().IsTouching(playerCollider))
        {
            if (Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.x) > killSpeedTreshold)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    public void createFoe()
    {
        float pickedHeight;
        do
        {
            pickedHeight = maxHeight * Random.value;
        } while (pickedHeight < GetComponent<SpriteRenderer>().bounds.size.y + 5);

        float sizeRatio = pickedHeight / GetComponent<SpriteRenderer>().bounds.size.y;
        Vector3 newSize = new Vector3();
        newSize = transform.localScale;
        newSize.y *= sizeRatio;
        transform.localScale = newSize;

        eyePlacement();

        BoxCollider2D colliderOne = gameObject.AddComponent<BoxCollider2D>();
        //colliderOne.bounds.extents.y -= 1;

    }

    void eyePlacement()
    {
        Vector3 newPosition = eye.transform.position;
        float newYPos = Random.value;
        Vector3 newSize = transform.localScale;

        do
        {
            newSize.y = Random.value * maxEyeSize;
            eye.transform.localScale = newSize;
        } while (eye.GetComponent<SpriteRenderer>().bounds.size.y < 0.1);
        float a;
        if (newYPos >= 0.5f)
        {
            newYPos = Random.value;
            newYPos = GetComponent<SpriteRenderer>().bounds.extents.y * newYPos;
            a = GetComponent<SpriteRenderer>().bounds.extents.y;
        }
        else
        {
            newYPos = Random.value;
            newYPos = GetComponent<SpriteRenderer>().bounds.extents.y * newYPos * -1;
            a = GetComponent<SpriteRenderer>().bounds.extents.y;
        }
        newPosition.y += newYPos;
        eye.transform.position = newPosition;
        //eye.GetComponent<SpriteRenderer>().bounds.extents.y
    }

    Quaternion rightfaceRotate(Vector3 direction)
    {
        return Quaternion.LookRotation(Vector3.forward, direction) * Quaternion.Euler(0, 0, 90);
    }

    void drawBetweenPoints(Vector3 from, Vector3 to, GameObject drawSubject)
    {
        Vector3 halfwayPoint = Vector3.Lerp(from, to, 0.5f);
        drawSubject.transform.position = halfwayPoint;
        float scaleCoef = Vector3.Distance(from, to) / armLength;
        Vector3 scaledSize = drawSubject.transform.localScale;
        scaledSize.x = scaleCoef;
        drawSubject.transform.localScale = scaledSize;
        Vector3 direction = from - to;
        drawSubject.transform.rotation = rightfaceRotate(direction);

    }


}
