using UnityEngine;
using System.Collections;

public class ObjectDestruction : MonoBehaviour
{
    public Transform enemy;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (enemy != null)
        {
            if (transform.position.x < enemy.position.x)
            {
                Destroy(gameObject);
            }
        }
    }
}
