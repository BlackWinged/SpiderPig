using UnityEngine;
using System.Collections;

public class ObjectDestructionEnemies : MonoBehaviour {
    private Transform enemy;
	// Use this for initialization
	void Start () {
        enemy = GameObject.FindGameObjectWithTag(Tags.CLEANER).transform;
	}
	
	// Update is called once per frame
	void Update () {
        recursiveDestroy();
    }

    void recursiveDestroy()
    {
        if (transform.position.x < enemy.position.x)
        {
            Destroy(transform.root.gameObject);
        }
    }
}
