using UnityEngine;
using System.Collections;

public class KillerControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetComponent<BoxCollider2D>().IsTouching(GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponentInChildren<CircleCollider2D>()))
        {
            Destroy(transform.parent.parent.gameObject);
        }
    }
}
