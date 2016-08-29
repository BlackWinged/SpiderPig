using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SegmentFactory : MonoBehaviour {
    public List<Transform> segments;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual List<Transform> getAvailableSegments()
    {
        return segments;
    }

}
