using UnityEngine;
using System.Collections;

public class BackupColliderControl : MonoBehaviour
{
    public float backupColliderWidth = 5;
    public LayerMask collideables;

    private BoxCollider2D backupCollider;
    private PlayerControl parent;
    // Use this for initialization
    void Start()
    {
        parent = gameObject.GetComponentInParent<PlayerControl>();
        //#################################
        backupCollider = gameObject.AddComponent<BoxCollider2D>();
        Vector3 size = new Vector3(parent.maxRopeLength, backupColliderWidth);
        backupCollider.size = size;
        backupCollider.isTrigger = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetComponentInParent<playerInput>().touch)
        {
            Vector2 distantPoint = GetComponentInParent<playerInput>().positions[0] - parent.transform.position;
            distantPoint *= 200;
            setBetweenTwoPoints(Vector3.MoveTowards(parent.transform.position, distantPoint,parent.maxRopeLength), parent.transform.position, gameObject);
        }
    }

    Quaternion rightfaceRotate(Vector3 direction)
    {
        return Quaternion.LookRotation(Vector3.forward, direction) * Quaternion.Euler(0, 0, 90);
    }

    void setBetweenTwoPoints(Vector3 from, Vector3 to, GameObject drawSubject)
    {
        Vector3 halfwayPoint = Vector3.Lerp(from, to, 0.5f);
        drawSubject.transform.position = halfwayPoint;
        Vector3 direction = from - to;
        drawSubject.transform.rotation = rightfaceRotate(direction);

    }
}
