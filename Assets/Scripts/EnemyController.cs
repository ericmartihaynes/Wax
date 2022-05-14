using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    private Rigidbody2D body;
    private float enemyHealth = 100f;
    private int shootCount = 0;
    public GameObject bulletPrefab;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth < 0) {
            this.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        
        Vector2 distance = this.transform.position - player.transform.position;
        if (shootCount > Random.Range(30, 200) && distance.magnitude < 18)
        {
            Vector2 playerVector = player.GetComponent<Rigidbody2D>().position;
            Vector2 playerToMouseVector = (playerVector - body.position).normalized;
            GameObject newBullet = Instantiate(bulletPrefab, body.position + playerToMouseVector, Quaternion.identity);
            newBullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(playerVector.y - transform.position.y, playerVector.x - transform.position.x) * Mathf.Rad2Deg);
            newBullet.GetComponent<Rigidbody2D>().AddForce(playerToMouseVector * 300, ForceMode2D.Impulse);
            Destroy(newBullet, 0.3f);


            shootCount = 0;
        }
        else {
            shootCount++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Metal")
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 damageVector = rb.velocity*rb.mass - body.velocity*body.mass;
            enemyHealth -= damageVector.magnitude / Random.Range(5, 30);
            Debug.Log(enemyHealth.ToString());
            if (rb.mass < 1.1f) {
                Destroy(collision.gameObject, 0.1f);
            }


        }
    }
}
