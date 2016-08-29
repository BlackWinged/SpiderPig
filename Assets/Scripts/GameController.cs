using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Transform player;
    public testGenerate generationClass;
    public Rigidbody2D bigBad;
    public float maxSpeed = 50;
    public Transform origSegment;

    public GameObject conversationObject;

    public bool hasBigBad = false;
    public bool isProcedural = false;

    private PlayerControl pc;
    private float oldPosition;
    private textParser textParserClass;
    private Object lastFiringClass;
    // Use this for initialization
    void Start()
    {
        if (player != null)
        {
            oldPosition = player.position.x;
            pc = player.GetComponent<PlayerControl>();
        }
        if (conversationObject != null)
        {
            textParserClass = conversationObject.GetComponent<textParser>();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasBigBad)
        {
            bigBadHandler();
        }

        if (isProcedural)
        {
            if (player.position.x - oldPosition > generationClass.square.GetComponent<BoxCollider2D>().bounds.size.x)
            {
                generationClass.setSegment();
                oldPosition = player.position.x;
                if (generationClass.currentPositionUpper.transform.position.x - player.position.x < 100)
                {
                    for (int i = 1; i < generationClass.segmentBlockNumber; i++)
                    {
                        generationClass.setSegment();
                    }
                }
            }
            origSegment.position = new Vector3(player.position.x, origSegment.position.y, 0);
        }

    }

    void bigBadHandler()
    {
        if (bigBad.velocity.magnitude < maxSpeed)
        {
              bigBad.velocity += new Vector2(0.1f, 0);
        }

    }

    public void killRopes()
    {
        pc.ropeDestruction();
    }

    public void fireConversation(string text)
    {
        textParserClass.beginSpeech(text);
    }


}
