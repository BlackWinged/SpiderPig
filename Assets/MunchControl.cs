using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MunchControl : MonoBehaviour {
    private Transform player;
    private CircleCollider2D playerCollider;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
        playerCollider = player.GetComponent<CircleCollider2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    if (GetComponent<BoxCollider2D>().IsTouching(playerCollider))
        {
           SceneManager.LoadScene("mainmenu");
        }
	}
}
