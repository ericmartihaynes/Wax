using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    private Rigidbody2D body;
    private float enemyHealth = 100f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth < 0) {
            this.gameObject.SetActive(false);
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
