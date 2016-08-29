using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class textParser : MonoBehaviour
{
    public float timeForLetter = 0.1f;

    private float timer;
    private string currentText = "";
    private List<string> textParagraphs = new List<string>();
    private IEnumerator paragraphEnum;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkForSpeech();
        writeOut();
    }

    void checkForSpeech()
    {
        string checkString = "";
        EventManager.checkForKey(EventKeys.CONVERSATION, out checkString);
        if (checkString != null)
        {
            if (!checkString.Equals(""))
            {
                EventManager.removeKey(EventKeys.CONVERSATION);
                beginSpeech(checkString);
            }

        }
    }

    public void beginSpeech(string text)
    {
        paragraphEnum = null;
        textParagraphs.Clear();
        textParagraphs.AddRange(text.Split('#'));
        GetComponent<Image>().enabled = true;
        paragraphEnum = textParagraphs.GetEnumerator();
        fireNextParagraph();
    }

    public void fireNextParagraph()
    {
        if (paragraphEnum != null)
        {
            if (!paragraphEnum.MoveNext())
            {
                GetComponent<Image>().enabled = false;
                GetComponentInChildren<Text>().text = "";
                currentText = "";
                paragraphEnum = null;
                textParagraphs.Clear();
            }
            else
            {
                GetComponentInChildren<Text>().text = "";
                currentText = (string)paragraphEnum.Current + "#";
            }
        }
    }
    void writeOut()
    {
        if (GetComponentInChildren<Text>().text.Length != currentText.Length - 1)
        {
            if (timer <= 0 && currentText.Length != 0)
            {
                GetComponentInChildren<Text>().text += currentText[GetComponentInChildren<Text>().text.Length];
            }
            timer -= Time.deltaTime;
        }
    }


}
