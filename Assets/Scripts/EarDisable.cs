using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EarDisable : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void earDisable(bool disable)
    {
        if (disable)
        {
            foreach (SpriteRenderer ear in GetComponentsInChildren<SpriteRenderer>()){
                ear.enabled = false;
                ear.GetComponent<Rigidbody2D>().mass = 0;
            }
        } else
        {
            foreach (SpriteRenderer ear in GetComponentsInChildren<SpriteRenderer>())
            {
                ear.enabled = true;
                ear.GetComponent<Rigidbody2D>().mass = 1;
            }
        }
    }
}
