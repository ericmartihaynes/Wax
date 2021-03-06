using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    
    public String sceneName;
    public Text textGO; 
    public void Setup(String textFinal)
    {
        textGO.text = textFinal;
        gameObject.SetActive(true);
        
    }
    
    public void RestartButton ()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
