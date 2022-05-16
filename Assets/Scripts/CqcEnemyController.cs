using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CqcEnemyController : MonoBehaviour
{
    private Rigidbody2D body;
    private LineRenderer line;
    private float enemyHealth = 100f;
    private int punchCount = 0;
    public GameObject punchPrefab;
    private GameObject player;
    private bool playerDetected = false;
    private float moveForce;
    private bool isFalling = true;
    public Animator animator;
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        moveForce = ((0.75f * body.mass) + 20f) / 4;
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 distance = this.transform.position - player.transform.position;
        if (enemyHealth < 0)
        {
            dead = true;
            animator.SetTrigger("Dead");
            StartCoroutine(deathCoroutine());
            enemyHealth = 10000000000000000000;
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
        if (distance.magnitude < 18) {
            playerDetected = true; 
        }
        if (punchCount > Random.Range(30, 60) && distance.magnitude < 1.5f && !dead)
        {
            playerDetected = true;
            Vector2 playerVector = player.GetComponent<Rigidbody2D>().position;
            Vector2 playerToMouseVector = (playerVector - body.position).normalized;
            float angle = Vector2.SignedAngle(new Vector2(0, 1), playerToMouseVector);
            GameObject newPunch = Instantiate(punchPrefab, body.position + playerToMouseVector, Quaternion.identity);
            newPunch.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(playerVector.y - transform.position.y, playerVector.x - transform.position.x) * Mathf.Rad2Deg);
            newPunch.GetComponent<Rigidbody2D>().AddForce(playerToMouseVector * 30, ForceMode2D.Impulse);
            Destroy(newPunch, 0.075f);
            switch (angle)
            {
                case float n when (0 < n && n < 180):

                    animator.SetTrigger("punchLeft");
                    break;
                case float n when (-180 < n && n < 0):

                    animator.SetTrigger("punchRight");
                    break;
            }
            SoundManagerScript.playSound("punch");

            punchCount = 0;
        }
        else
        {
            punchCount++;
        }

        if (playerDetected && distance.magnitude > 1.5f && isFalling == false && !dead)
        {
            int rightOrLeft;
            if (player.transform.position.x - this.transform.position.x < 0) { rightOrLeft = -1; }
            else { rightOrLeft = 1; }
            animator.SetInteger("Walking", rightOrLeft);
            body.AddForce(new Vector2(rightOrLeft * moveForce, 0f), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            isFalling = false;
        }

        if (collision.gameObject.tag == "Metal" || collision.gameObject.tag == "Punch")
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 damageVector = rb.velocity * rb.mass - body.velocity * body.mass;
            enemyHealth -= damageVector.magnitude / Random.Range(5, 30);
            SoundManagerScript.playSound("hitmarker");
            Debug.Log(enemyHealth.ToString());
            if (rb.mass < 1.1f)
            {
                Destroy(collision.gameObject, 0.1f);
            }


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            isFalling = true;
        }
    }

    private IEnumerator deathCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);
    }
}
