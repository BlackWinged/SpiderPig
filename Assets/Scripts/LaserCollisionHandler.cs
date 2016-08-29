using UnityEngine;
using System.Collections;

public class LaserCollisionHandler : MonoBehaviour {
    public LayerMask Ground;
    public LayerMask Player;
    public float maxRange;

    private Vector3 positionOld;
    private float distance;
  
    // Use this for initialization
    void Start() {
        positionOld = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate() {
        
        distance += Vector2.Distance(transform.position, positionOld);
        if (GetComponent<PolygonCollider2D>().IsTouchingLayers(Ground) || distance > maxRange)
        {
            Destroy(gameObject);
        }
        positionOld = transform.position;
    }

   


}
