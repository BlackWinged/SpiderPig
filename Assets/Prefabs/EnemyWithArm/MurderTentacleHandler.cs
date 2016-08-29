using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MurderTentacleHandler : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetComponent<BoxCollider2D>().IsTouching(GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponentInChildren<CircleCollider2D>()))
        {
            SceneManager.LoadScene("mainmenu");
        }
    }


}
