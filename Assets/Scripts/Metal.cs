using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : MonoBehaviour
{
    private Color lineBlue = new Color(0.5f, 0.92f, 1f, 1f);
    private LineRenderer line;
    private GameObject player;
    private Rigidbody2D body;
    public bool isCoin;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        body = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 distance = this.transform.position - player.transform.position;
        if (distance.magnitude < 20 && player.GetComponent<PlayerController>().metalVision)
        {
            line.startWidth = 0.05f;
            line.endWidth = 0.05f;
            line.startColor = new Color(0.5f, 0.92f, 1f, ((body.mass / 100f) * ((20 - distance.magnitude) / 20)) + 0.2f);
            line.endColor = new Color(0.5f, 0.92f, 1f, ((body.mass / 100f) * ((20 - distance.magnitude) / 20)) + 0.2f);
            line.SetPosition(0, (this.transform.position + new Vector3(0, 0, -1)));
            line.SetPosition(1, (player.transform.position + new Vector3(0, 0, -1)));
        }
        else {
            line.startColor = new Color(0.5f, 0.92f, 1f, 0f);
            line.endColor = new Color(0.5f, 0.92f, 1f, 0f);
            
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCoin && collision.gameObject.tag == "Platform") {
            body.mass = 60; 
        }
    }

    
}
