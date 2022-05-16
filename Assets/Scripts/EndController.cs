using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class EndController : MonoBehaviour
{
    public GameOverScript gameOver;
    
   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameOver.Setup("YOU Win!");
        }
    }
}
