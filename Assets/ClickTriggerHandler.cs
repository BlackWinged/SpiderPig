using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickTriggerHandler : MonoBehaviour {

    public LayerMask triggerables;
    public string text;
    public List<GameObject> toDeactivate;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<BoxCollider2D>().IsTouchingLayers(triggerables))
        {
            EventManager.setEvent(EventKeys.CONVERSATION, null, text);
            GetComponent<BoxCollider2D>().enabled = false;
            deactivate();
        }
    }

    void OnMouseDown()
    {
        EventManager.setEvent(EventKeys.CONVERSATION, null, text);
        GetComponent<BoxCollider2D>().enabled = false;
        deactivate();
    }

    public void deactivate()
    {
        foreach (GameObject obj in toDeactivate)
        {
            obj.SetActive(false);
        }
    }

}

