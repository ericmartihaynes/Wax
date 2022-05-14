using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position;   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.GetComponent<PlayerController>().metalBubble && collision.gameObject.name.Substring(0,6) == "Bullet") {
            Rigidbody2D bulletBody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 push = (collision.gameObject.transform.position - this.transform.position);
            bulletBody.AddForce(push * 108, ForceMode2D.Impulse);

        }
    }

}
