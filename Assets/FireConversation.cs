using UnityEngine;
using System.Collections;

public class FireConversation : MonoBehaviour {
    public LayerMask triggerables;
    public string text;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	    if (GetComponent<BoxCollider2D>().IsTouchingLayers(triggerables))
        {
            EventManager.setEvent(EventKeys.CONVERSATION, null, text);
            GetComponent<BoxCollider2D>().enabled = false;
        }
	}


}
