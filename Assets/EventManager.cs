using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

public class EventManager : MonoBehaviour {

    private static EventManager eventmngr;
    public static EventManager instance
    {
        get
        {
            if (!eventmngr)
            {
                eventmngr = FindObjectOfType(typeof(EventManager)) as EventManager;
            }
            if (!eventmngr)
            {
                Debug.LogError("No active event managers loaded in scene");
            }
            return eventmngr;
        }
    }

    public Dictionary<string, UnityEvent> events = new Dictionary<string, UnityEvent>();
    public Dictionary<string, string> eventParams = new Dictionary<string, string>();

    public static void setEvent(string key, UnityAction action)
    {
        if (!instance.events.ContainsKey(key))
        {
            instance.events.Add(key, new UnityEvent());
        }
        instance.events[key].RemoveAllListeners();
        instance.events[key].AddListener(action);
    }

    public static void setEvent(string key, UnityAction action, string param)
    {
        setEvent(key, action);
        if (!instance.eventParams.ContainsKey(key))
        {
            instance.eventParams.Add(key, "");
        }
        instance.eventParams[key] = param;
    }

    public static bool checkForKey(string key, out string param)
    {
        param = "";
        bool result = false;
        if (instance.events.ContainsKey(key) || instance.eventParams.ContainsKey(key))
        {
            result = true;
        }
        if (param.Equals("")) instance.eventParams.TryGetValue(key, out param);
        return result;
    }

    public static bool checkForKey(string key)
    {
        string nullString;
        return checkForKey(key, out nullString);
    }

    public static void removeKey(string key)
    {
        if (instance.events.ContainsKey(key))
        {
            instance.events.Remove(key);
        }
        if (instance.eventParams.ContainsKey(key))
        {
            instance.eventParams.Remove(key);
        }
    }
}
