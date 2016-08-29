using UnityEngine;
using System.Collections;

public class ShootingFoeController : MonoBehaviour
{

    public Transform laser;
    public float shotInterval;
    public float maxShotSpeed;
    public float reactionDistance = 30;

    private Transform player;
    private CircleCollider2D playerCollider;
    private float Timer;
    private GameObject gc;
    private GameController gc_class;
    private Vector3 startPosition;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
        playerCollider = player.GetComponentInChildren<CircleCollider2D>();
        gc = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER);
        gc_class = gc.GetComponent<GameController>();
        Timer = shotInterval;
        startPosition = transform.position;
      //  GetComponent<SpringJoint2D>().connectedAnchor = transform.position + new Vector3(0,30,0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            if (Vector2.Distance(player.position, transform.position) < reactionDistance)
            {
                fire();
            }
            Timer = shotInterval;
        }
        if (GetComponent<PolygonCollider2D>().IsTouching(playerCollider) || GetComponent<SpringJoint2D>() == null)
        {
            death();
        }
    }

    void fire()
    {
        Transform shot = (Transform)Instantiate(laser);
        shot.position = transform.position;
        Vector2 velocity = calculateLeadingVelocity(2);
        shot.rotation = rightfaceRotate(velocity);
        shot.GetComponent<Rigidbody2D>().velocity = velocity * 300;
        shot.GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(shot.GetComponent<Rigidbody2D>().velocity, maxShotSpeed);

    }

    Vector2 calculateLeadingVelocity(int iterations)
    {
        float timeFlight = 0;
        Vector2 target = (Vector2)player.position;
        for (int i = 0; i < iterations; i++)
        {
            timeFlight = Vector2.Distance(transform.position, target) / maxShotSpeed;
            target = target + (player.GetComponent<Rigidbody2D>().velocity * timeFlight);
        }
        target = (Vector2)player.position;
        target = target + (player.GetComponent<Rigidbody2D>().velocity * timeFlight);

        return target - (Vector2)transform.position;
    }

    Quaternion rightfaceRotate(Vector3 direction)
    {
        return Quaternion.LookRotation(Vector3.forward, direction) * Quaternion.Euler(0, 0, 90);
    }

    void death()
    {
        if (player.GetComponents<DistanceJoint2D>().Length < 2)
        {
            if (player.GetComponent<DistanceJoint2D>().connectedBody != null)
            {
                if (player.GetComponent<DistanceJoint2D>().connectedBody.Equals(GetComponent<Rigidbody2D>()))
                {
                    gc_class.killRopes();
                }
            }
        }
        Destroy(gameObject);
    }

}
