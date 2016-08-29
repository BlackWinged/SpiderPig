using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    public void startRaceScene()
    {
        SceneManager.LoadScene("agile");
    }

    public void startInstantDeath()
    {
        SceneManager.LoadScene("suddendeath");
    }

    public void startTutorial()
    {
        SceneManager.LoadScene("tutorial");
    }
    public void startMenu()
    {
        SceneManager.LoadScene("mainmenu");
    }
}
