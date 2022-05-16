using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Level1() {
        SceneManager.LoadScene("TestLevel");
    }

    public void Exit() {
        Application.Quit();
    }
}
