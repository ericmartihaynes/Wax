using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    public float moveForce;
    public float jumpForce;
    public bool isJumping;//!!!
    private float inputHorizontal;
    private float inputVertical;
    private float inputScroll;
    private bool inputSpace;
    private int inputCoin = 0;
    private int inputVial = 0;
    private int inputBullet = 0;
    private int inputResetWeight = 0;
    private int inputPunch = 0;
    public float maxSpeed;
    private bool isFalling = true;
    private GameObject[] metals;
    private GameObject[] enemies;
    public float steelBurningRate = 1f;
    //private float equipmentMass = 3.5f;
    private int bullets = 36;
    private int coins = 10;
    private int metalVials = 3;
    private int bulletCasings = 0;
    private float metalReserve = 1000;
    private float massReserve = 0;
    private float normalMass = 80;
    private float currentEquipmentMass;
    private float health = 100;
    public bool metalVision = false;
    private bool visionState = false;
    private bool visionState2 = false;
    public bool metalBubble = false;
    public GameObject coinPrefab;
    public GameObject casingPrefab;
    public GameObject bulletPrefab;
    public GameObject punchPrefab;
    private int prefabCleaner = 0;
    public Text textBullets;
    public Text textCoins;
    public Text textCasings;
    public Text textVials;
    public Text textCurrentMass;
    public Text textStoredMass;
    public Text textMetalReserve;
    public Text textHealth;
    public Text textGameOver;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        metals = GameObject.FindGameObjectsWithTag("Metal");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        textBullets.text = bullets.ToString();
        textCoins.text = coins.ToString();
        textCasings.text = bulletCasings.ToString();
        textVials.text = metalVials.ToString();
        textMetalReserve.text = metalReserve.ToString();
        textHealth.text = health.ToString();
        textGameOver.text = "";


    }

    // Update is called once per frame
    void Update()
    {
        //metals = GameObject.FindGameObjectsWithTag("Metal");

        if (health < 0)
        {
            textGameOver.text = "Game Over";
        }
        if (prefabCleaner > 2500)
        {
            metals = GameObject.FindGameObjectsWithTag("Metal");
            foreach (GameObject metal in metals)
            {

                if (metal.GetComponent<Metal>().isCoin)
                {
                    Vector2 distance = metal.transform.position - this.transform.position;
                    if (distance.magnitude > 50)
                    {
                        Destroy(metal, 1);
                        
                    }
                }
            }

            prefabCleaner = 0;
        }
        else { prefabCleaner++; }

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        inputScroll += Input.GetAxis("Mouse ScrollWheel");
        inputSpace = Input.GetButton("Jump");
        if (Input.GetButtonDown("Fire1"))
        {
            inputBullet++;
        }
        if (Input.GetButtonDown("Fire3"))
        {
            inputResetWeight++;
        }

        if (Input.GetKeyDown("e")) {
            inputCoin++;
        }

        if (Input.GetKeyDown("q"))
        {
            inputVial++;
        }

        if (Input.GetKeyDown("c"))
        {
            inputPunch++;
        }

        if (Input.GetKeyDown("r") && metalReserve > 0)
        {
            if (!metalBubble)
            {
                metalVision = !metalVision;
                visionState = !visionState;
                visionState2 = !visionState2;
            }
        }
        if (Input.GetKeyDown("f") && metalReserve > 0)
        {
            metalBubble = !metalBubble;
            visionState2 = !visionState2;
            metalVision = true;
            if (!visionState && !visionState2) { metalVision = visionState; }
        }
        if (metalReserve < 0) {
            metalVision = false;
            metalBubble = false;
        }


    }

    void FixedUpdate()
    {
        
        GetComponent<Rigidbody2D>().mass -= currentEquipmentMass;
        float currentMass = GetComponent<Rigidbody2D>().mass;
        //currentEquipmentMass = equipmentMass + bullets * 0.007f + coins * 0.005f + metalVials * 0.05f + bulletCasings * 0.004f;
        currentEquipmentMass = 4f;
        float newMass = GetComponent<Rigidbody2D>().mass + inputScroll * (currentMass / 10) + currentEquipmentMass;

        if (massReserve < 0)
        {
            GetComponent<Rigidbody2D>().mass = normalMass + currentEquipmentMass;
            massReserve = 0;
        }
        else
        {
            if (newMass < normalMass + currentEquipmentMass)
            {
                GetComponent<Rigidbody2D>().mass = newMass;
                massReserve += (normalMass - newMass) / 1000;
            }
            else if (newMass > normalMass + currentEquipmentMass + 1f)
            {
                if (newMass > 8000f) { newMass = 8000f; }
                GetComponent<Rigidbody2D>().mass = newMass;
                massReserve -= ((newMass) - normalMass) / 1000;

            }
            else {
                GetComponent<Rigidbody2D>().mass = newMass;
            }
        }
        if (massReserve < 0)
        {
            GetComponent<Rigidbody2D>().mass = normalMass + currentEquipmentMass;
            massReserve = 0;
        }

        textStoredMass.text = massReserve.ToString();
        textCurrentMass.text = GetComponent<Rigidbody2D>().mass.ToString();
        inputScroll = 0;
        if (isFalling){
            GetComponent<Rigidbody2D>().drag = 0.5f;
        }
        else {
            GetComponent<Rigidbody2D>().drag = 4;
        }



        //Equation of mass to movement force y=0.75x+20
        moveForce = (0.75f * currentMass) + 20f;
        //Equation of mass to jump force y=6x+300
        jumpForce = ((6f * currentMass) + 300f);

        if (inputHorizontal != 0f && !isFalling && body.velocity.magnitude < maxSpeed)
        {
            body.AddForce(new Vector2(inputHorizontal * moveForce, 0f), ForceMode2D.Impulse);
            if (body.velocity.magnitude > maxSpeed)
            {
                body.velocity = Vector2.ClampMagnitude(body.velocity, maxSpeed);
            }
            
        }
        
        if (inputVertical != 0f && !isFalling)
        {
            body.AddForce(new Vector2(0f, inputVertical * jumpForce), ForceMode2D.Impulse);
            //might need to be fixed for situations where the player is moved by external force and jumps
            if (body.velocity.magnitude > maxSpeed + 2)
            {
                body.velocity = new Vector2(body.velocity.x, maxSpeed);
                
            }
        }

        if (inputSpace && metalReserve > 0) {
            metalVision = true;
            metals = GameObject.FindGameObjectsWithTag("Metal");
            foreach (GameObject metal in metals) {
                Vector2 push = (metal.transform.position - this.transform.position);
                if (push.magnitude < 20) { //Range at which push has effect
                    push = push.normalized * ((20 - push.magnitude) / 20);
                    Rigidbody2D metalBody = metal.GetComponent<Rigidbody2D>();
                    Vector2 push2 = push * steelBurningRate * metalBody.mass * -1;
                    push = push * steelBurningRate * currentMass;
                    metalBody.AddForce(push, ForceMode2D.Impulse);
                    body.AddForce(push2, ForceMode2D.Impulse);

                    metalReserve -= (push.magnitude + push2.magnitude) / 100;
                    if (metalReserve < 0 ) { metalReserve = 0; }
                    textMetalReserve.text = metalReserve.ToString();
                }
            }

            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Vector2 push = (enemy.transform.position - this.transform.position);
                if (push.magnitude < 20)
                { //Range at which push has effect
                    push = push.normalized * ((20 - push.magnitude) / 20);
                    Rigidbody2D metalBody = enemy.GetComponent<Rigidbody2D>();
                    Vector2 push2 = push * steelBurningRate * 5 * -1;
                    push = push * steelBurningRate * 5;
                    metalBody.AddForce(push, ForceMode2D.Impulse);
                    body.AddForce(push2, ForceMode2D.Impulse);

                    metalReserve -= (push.magnitude + push2.magnitude) / 100;
                    if (metalReserve < 0) { metalReserve = 0; }
                    textMetalReserve.text = metalReserve.ToString();
                }
            }
            
        }
        else {
            if (!visionState && !visionState2) { metalVision = visionState; }
            
        }

        if (inputResetWeight > 0)
        {
            GetComponent<Rigidbody2D>().mass = normalMass + currentEquipmentMass;
            textCurrentMass.text = GetComponent<Rigidbody2D>().mass.ToString();
            inputResetWeight = 0;
        }

        if (inputBullet > 0 && bullets > 0)
        {
            Vector2 coinVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerToMouseVector = (coinVector - body.position).normalized;
            GameObject newBullet = Instantiate(bulletPrefab, body.position + playerToMouseVector, Quaternion.identity);
            newBullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(coinVector.y - transform.position.y, coinVector.x - transform.position.x) * Mathf.Rad2Deg);
            newBullet.GetComponent<Rigidbody2D>().AddForce(playerToMouseVector * 300, ForceMode2D.Impulse);
            body.AddForce(playerToMouseVector * -10, ForceMode2D.Impulse);
            metals = GameObject.FindGameObjectsWithTag("Metal");
            bullets--;
            bulletCasings++;
            textBullets.text = bullets.ToString();
            textCasings.text = bulletCasings.ToString();
            inputBullet = 0;
            inputCoin = 0; //Dont know why but if I remove this random casing appears

        }

        if (inputCoin > 0 && (coins > 0 || bulletCasings > 0))
        {
            
            Vector2 coinVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerToMouseVector = (coinVector - body.position).normalized;
            GameObject newCoin;
            if (bulletCasings > 0) {
                newCoin = Instantiate(casingPrefab, body.position + playerToMouseVector, Quaternion.identity);
                bulletCasings--;
                textCasings.text = bulletCasings.ToString();
            }
            else {
                newCoin = Instantiate(coinPrefab, body.position + playerToMouseVector, Quaternion.identity);
                coins--;
                textCoins.text = coins.ToString();
            }
            
            newCoin.GetComponent<Rigidbody2D>().AddTorque(0.5f);
            newCoin.GetComponent<Rigidbody2D>().AddForce(playerToMouseVector * 3, ForceMode2D.Impulse);
            metals = GameObject.FindGameObjectsWithTag("Metal");
            inputCoin = 0;
            
        }

        if (inputVial > 0 && metalVials > 0 && metalReserve < 1000)
        {
            metalVials--;
            metalReserve += 800;
            textMetalReserve.text = metalReserve.ToString();
            textVials.text = metalVials.ToString();
            inputVial = 0;
        }

        if (inputPunch > 0)
        {
            Vector2 coinVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerToMouseVector = (coinVector - body.position).normalized / 2;
            GameObject newPunch = Instantiate(punchPrefab, body.position + playerToMouseVector, Quaternion.identity);
            newPunch.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(coinVector.y - transform.position.y, coinVector.x - transform.position.x) * Mathf.Rad2Deg);
            newPunch.GetComponent<Rigidbody2D>().AddForce(playerToMouseVector * (currentMass / 3), ForceMode2D.Impulse);
            Destroy(newPunch,0.085f);
            inputPunch = 0;
        }
        if (metalVision) {
            metalReserve -= 0.005f;
            textMetalReserve.text = metalReserve.ToString();
        }
        if (metalBubble)
        {
            metalReserve -= 0.1f;
            textMetalReserve.text = metalReserve.ToString();
        }

        //DONE: simple ui, make weight and metal limited
        //DONE: Add Kinematic metal
        //DONE: Expand test area, fix camera
        //DONE: Add enemies, enemy health, player health, and physics based damage system, shooting, random misses
        //DONE: Add enemy movement, CQC player, CQC enemies, Aluminum enemies, Metal on enemies
        //DONE: Metal vision ability
        //DONE: Metal push shield ability
        //DONE: Enemy that does not move
        //TODO: Player and enemy sprites and animations
        //TODO: Health to 0 if you fall too far
        //TODO: Enemies drop items
        //TODO: Sounds
        //TODO: Improve UI
        //TODO: Platform that breaks under weight
        //TODO: Boss Enemy
        //TODO: Maybe select only one metal

        //TODO: Add other things (see itslearning)
        //TODO: Refactor
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform" ) {
            isFalling = false;
        }

        if (collision.gameObject.tag == "Metal" || collision.gameObject.tag == "Aluminum")
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 damageVector = rb.velocity * rb.mass - body.velocity * body.mass;
            float damage = damageVector.magnitude / Random.Range(5, 30);
            if (damage > 10) {
                health -= damage;
                textHealth.text = health.ToString();
            }
            if (rb.mass < 1.1f)
            {
                Destroy(collision.gameObject, 0.1f);
            }


        }
        if (collision.gameObject.tag == "EnemyPunch")
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 damageVector = rb.velocity * rb.mass - body.velocity * body.mass;
            float damage = damageVector.magnitude / Random.Range(5, 30);
            health -= damage;
            textHealth.text = health.ToString();
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            isFalling = true;
        }
    }
}
