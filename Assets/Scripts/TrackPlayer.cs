using UnityEngine;
using System.Collections;

public class TrackPlayer : MonoBehaviour
{

    public Transform target;
    public float smoothing = 5f;
    public EarDisable leftEars;
    public EarDisable rightEars;
    // Use this for initialization

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 speedBuffCheck = target.GetComponent<Rigidbody2D>().velocity;
        Vector3 targetCamPos = target.position;
        targetCamPos.z = -10;
        Vector3 euleraAngle = Quaternion.LookRotation(Vector3.forward, speedBuffCheck).eulerAngles;
        float zAxis = euleraAngle.z;
        if (target.GetComponent<Rigidbody2D>().velocity.magnitude > 1)
        {
            if ((zAxis > 0) && (zAxis < 180))
            {
                transform.position = Vector3.Lerp(transform.position, targetCamPos + Vector3.left * 10, speedBuffCheck.magnitude / 10 * Time.deltaTime);
                leftEars.earDisable(true);
                rightEars.earDisable(false);
               // target.GetComponent<PlayerControl>().extendRope(false);

            }
            if ((zAxis > 180) && (zAxis < 360))
            {
                transform.position = Vector3.Lerp(transform.position, targetCamPos + Vector3.right * 10, speedBuffCheck.magnitude / 10 * Time.deltaTime);
                leftEars.earDisable(false);
                rightEars.earDisable(true);
               // target.GetComponent<PlayerControl>().extendRope(false);
            }
        }
        if (target.GetComponent<Rigidbody2D>().velocity.magnitude < 1 || zAxis == 180)
        {
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}

