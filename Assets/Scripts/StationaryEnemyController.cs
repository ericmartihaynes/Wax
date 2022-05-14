using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StationaryEnemyController : MonoBehaviour
{
    private Rigidbody2D body;
    private LineRenderer line;
    private float enemyHealth = 100f;
    private int shootCount = 0;
    public GameObject bulletPrefab;
    private GameObject player;
    private bool playerDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 distance = this.transform.position - player.transform.position;
        if (enemyHealth < 0)
        {
            this.gameObject.SetActive(false);
        }
        if (distance.magnitude < 20 && player.GetComponent<PlayerController>().metalVision)
        {
            line.startWidth = 0.05f;
            line.endWidth = 0.05f;
            line.startColor = new Color(0.5f, 0.92f, 1f, ((5 / 100f) * ((20 - distance.magnitude) / 20)) + 0.2f);
            line.endColor = new Color(0.5f, 0.92f, 1f, ((5 / 100f) * ((20 - distance.magnitude) / 20)) + 0.2f);
            line.SetPosition(0, (this.transform.position + new Vector3(0, 0, -1)));
            line.SetPosition(1, (player.transform.position + new Vector3(0, 0, -1)));
        }
        else
        {
            line.startColor = new Color(0.5f, 0.92f, 1f, 0f);
            line.endColor = new Color(0.5f, 0.92f, 1f, 0f);

        }
    }

    void FixedUpdate()
    {

        Vector2 distance = this.transform.position - player.transform.position;
        if (shootCount > Random.Range(30, 200) && distance.magnitude < 18)
        {
            playerDetected = true;
            Vector2 playerVector = player.GetComponent<Rigidbody2D>().position;
            playerVector.x = playerVector.x + Random.Range(-1.5f, 1.5f);
            playerVector.y = playerVector.y + Random.Range(-1.5f, 1.5f);
            Vector2 playerToMouseVector = (playerVector - body.position).normalized;
            GameObject newBullet = Instantiate(bulletPrefab, body.position + playerToMouseVector, Quaternion.identity);
            newBullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(playerVector.y - transform.position.y, playerVector.x - transform.position.x) * Mathf.Rad2Deg);
            newBullet.GetComponent<Rigidbody2D>().AddForce(playerToMouseVector * 300, ForceMode2D.Impulse);
            Destroy(newBullet, 0.3f);


            shootCount = 0;
        }
        else
        {
            shootCount++;
        }

        if (playerDetected && distance.magnitude > 18)
        {
            playerDetected = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.gameObject.tag == "Metal" || collision.gameObject.tag == "Punch")
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 damageVector = rb.velocity * rb.mass - body.velocity * body.mass;
            enemyHealth -= damageVector.magnitude / Random.Range(5, 30);
            Debug.Log(enemyHealth.ToString());
            if (rb.mass < 1.1f)
            {
                Destroy(collision.gameObject, 0.1f);
            }


        }
    }

    
}
