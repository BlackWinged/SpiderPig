using UnityEngine;
using System.Collections;

public class SlowDown : MonoBehaviour
{
    private CircleCollider2D player;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponentInChildren<CircleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Time.timeScale = 1f;
        if (GetComponent<BoxCollider2D>().IsTouching(player))
        {
            Time.timeScale = 0.5f;
        }
    }
}
