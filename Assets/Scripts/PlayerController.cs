using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    public float moveForce;
    public float jumpForce;
    public bool isJumping; //!!!
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
    //public Text textGameOver;
    public Animator animator;
    private AudioSource[] audioS;
    private int movingDirection;
    private bool jumped = false;
    private VolumeManager vol;
    public Bars healthBar;
    public Bars metalBar;
    public Bars weightBar;
    public GameObject iconVision;
    public GameObject iconBubble;
    public GameObject iconMetal;
    public GameObject iconWeight;
    public GameOverScript gameOver;
    private bool gameover = false;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        metals = GameObject.FindGameObjectsWithTag("Metal");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        animator = GetComponent<Animator>();
        audioS = GetComponents<AudioSource>();
        /*foreach (AudioSource audio in audioS)
        {
            vol.volumenManager(audio);
        }*/
        textBullets.text = bullets.ToString();
        textCoins.text = coins.ToString();
        textCasings.text = bulletCasings.ToString();
        textVials.text = metalVials.ToString();
        
    }

    private void GameOver()
    {
        gameOver.Setup("Game Over");
    }
    // Update is called once per frame
    void Update()
    {
        if (metalVision) { iconVision.GetComponent<Image>().color = new Color(0.5f, 0.92f, 1f); }
        else { iconVision.GetComponent<Image>().color = new Color(1f, 1f, 1f); }
        if (metalBubble) { iconBubble.GetComponent<Image>().color = new Color(0.5f, 0.92f, 1f); }
        else { iconBubble.GetComponent<Image>().color = new Color(1f, 1f, 1f); }

        healthBar.SetValue(health);
        if (!gameover && health < 0 || (this.transform.position.y < -50) && (this.transform.position.y > -51))
        {
            gameover = true;
           // health = 10000000000000000000;
           animator.SetTrigger("Dead");
            SoundManagerScript.playSound("death");
            Invoke("GameOver",3f);
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
        else
        {
            prefabCleaner++;
        }

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        if (inputHorizontal < 0)
        {
            movingDirection = -1;
            animator.SetInteger("Walking", movingDirection);
            sounds(0);
        }
        else if (inputHorizontal > 0)
        {
            movingDirection = 1;
            animator.SetInteger("Walking", movingDirection);
            sounds(0);
        }
        else
        {
            movingDirection = 0;
            animator.SetInteger("Walking", movingDirection);
            sounds(0);
        }


        inputVertical = Input.GetAxisRaw("Vertical");
        if (inputVertical != 0 && !jumped)
        {
            animator.SetTrigger("Jump");
            jumped = true;
            animator.SetBool("Falling", true);
        }
        
        inputScroll += Input.GetAxis("Mouse ScrollWheel");
        if (inputScroll < 0) { iconWeight.GetComponent<Image>().color = new Color(0.5f, 0.92f, 1f); }
        else if (inputScroll > 0) { iconWeight.GetComponent<Image>().color = new Color(1f, 0.5f, 0.5f); }
        else if (GetComponent<Rigidbody2D>().mass == 84) { iconWeight.GetComponent<Image>().color = new Color(1f, 1f, 1f); }
        inputSpace = Input.GetButton("Jump");
        sounds(1);
        if (Input.GetButtonDown("Fire1"))
        {
            inputBullet++;
        }

        if (Input.GetButtonDown("Fire3"))
        {
            inputResetWeight++;
        }

        if (Input.GetKeyDown("e"))
        {
            inputCoin++;
        }

        if (Input.GetKeyDown("q"))
        {
            inputVial++;
        }

        if (Input.GetButtonDown("Fire2"))
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
            if (!visionState && !visionState2)
            {
                metalVision = visionState;
            }
        }

        if (metalReserve < 0)
        {
            metalVision = false;
            metalBubble = false;
        }
    }

    private void sounds(int i)
    {
        if (movingDirection != 0 && i == 0)
        {
            if (!audioS[i].isPlaying)
            {
                audioS[i].Play();
            }
        }
        else if (inputSpace && i == 1)
        {
            if (!audioS[i].isPlaying)
            {
                audioS[i].Play();
            }
        }
        else
        {
            audioS[i].Stop();
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
                if (newMass > 8000f)
                {
                    newMass = 8000f;
                }

                GetComponent<Rigidbody2D>().mass = newMass;
                massReserve -= ((newMass) - normalMass) / 1000;
            }
            else
            {
                GetComponent<Rigidbody2D>().mass = newMass;
            }
        }

        if (massReserve < 0)
        {
            GetComponent<Rigidbody2D>().mass = normalMass + currentEquipmentMass;
            massReserve = 0;
        }

        textCurrentMass.text = Mathf.Round(GetComponent<Rigidbody2D>().mass).ToString();
        weightBar.SetValue(massReserve);
        inputScroll = 0;
        if (isFalling)
        {
            GetComponent<Rigidbody2D>().drag = 0.5f;
        }
        else
        {
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
            SoundManagerScript.playSound("jump");
            if (body.velocity.magnitude > maxSpeed + 2)
            {
                body.velocity = new Vector2(body.velocity.x, maxSpeed);
            }
        }

        if (inputSpace && metalReserve > 0)
        {
            metalVision = true;
            metals = GameObject.FindGameObjectsWithTag("Metal");
            foreach (GameObject metal in metals)
            {
                Vector2 push = (metal.transform.position - this.transform.position);
                if (push.magnitude < 20)
                {
                    //Range at which push has effect
                    push = push.normalized * ((20 - push.magnitude) / 20);
                    Rigidbody2D metalBody = metal.GetComponent<Rigidbody2D>();
                    Vector2 push2 = push * steelBurningRate * metalBody.mass * -1;
                    push = push * steelBurningRate * currentMass;
                    metalBody.AddForce(push, ForceMode2D.Impulse);
                    body.AddForce(push2, ForceMode2D.Impulse);

                    metalReserve -= (push.magnitude + push2.magnitude) / 100;
                    if (metalReserve < 0)
                    {
                        metalReserve = 0;
                        metalBar.SetValue(metalReserve);
                    }

                }
            }

            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Vector2 push = (enemy.transform.position - this.transform.position);
                if (push.magnitude < 20)
                {
                    //Range at which push has effect
                    push = push.normalized * ((20 - push.magnitude) / 20);
                    Rigidbody2D metalBody = enemy.GetComponent<Rigidbody2D>();
                    Vector2 push2 = push * steelBurningRate * 5 * -1;
                    push = push * steelBurningRate * 5;
                    metalBody.AddForce(push, ForceMode2D.Impulse);
                    body.AddForce(push2, ForceMode2D.Impulse);

                    metalReserve -= (push.magnitude + push2.magnitude) / 100;
                    if (metalReserve < 0)
                    {
                        metalReserve = 0;
                    }

                    metalBar.SetValue(metalReserve);
                }
            }
            iconMetal.GetComponent<Image>().color = new Color(0.5f, 0.92f, 1f);


        }
        else
        {
            if (!visionState && !visionState2)
            {
                metalVision = visionState;
            }
            iconMetal.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }

        if (inputResetWeight > 0)
        {
            GetComponent<Rigidbody2D>().mass = normalMass + currentEquipmentMass;
            textCurrentMass.text = Mathf.Round(GetComponent<Rigidbody2D>().mass).ToString();
            inputResetWeight = 0;
            iconWeight.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            SoundManagerScript.playSound("weightPower");
        }

        if (inputBullet > 0 && bullets > 0)
        {
            Vector2 coinVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerToMouseVector = (coinVector - body.position).normalized;
            float angle = Vector2.SignedAngle(new Vector2(0,1), playerToMouseVector);
            GameObject newBullet = Instantiate(bulletPrefab, body.position + playerToMouseVector, Quaternion.identity);
            newBullet.transform.rotation = Quaternion.Euler(0, 0,
                Mathf.Atan2(coinVector.y - transform.position.y, coinVector.x - transform.position.x) * Mathf.Rad2Deg);
            newBullet.GetComponent<Rigidbody2D>().AddForce(playerToMouseVector * 300, ForceMode2D.Impulse);
            body.AddForce(playerToMouseVector * -10, ForceMode2D.Impulse);
            metals = GameObject.FindGameObjectsWithTag("Metal");
            bullets--;
            SoundManagerScript.playSound("fire");
            bulletCasings++;
            textBullets.text = bullets.ToString();
            textCasings.text = bulletCasings.ToString();
            inputBullet = 0;
            inputCoin = 0; //Dont know why but if I remove this random casing appears
            Debug.Log(angle.ToString());
            switch (angle)
            {
                case float n when (0 < n && n < 16):
                    
                    animator.SetTrigger("shootLeft5");
                    break;
                case float n when (16 < n && n < 70):
                    
                    animator.SetTrigger("shootLeft4");
                    break;
                case float n when (70 < n && n < 110):
                    
                    animator.SetTrigger("shootLeft3");
                    break;
                case float n when (110 < n && n < 150):
                    
                    animator.SetTrigger("shootLeft2");
                    break;
                case float n when (150 < n && n < 180):
                    
                    animator.SetTrigger("shootLeft1");
                    break;
                case float n when (-16 < n && n < 0):
                    
                    animator.SetTrigger("shootRight5");
                    break;
                case float n when (-70 < n && n < -16):
                    
                    animator.SetTrigger("shootRight4");
                    break;
                case float n when (-110 < n && n < -70):
                    
                    animator.SetTrigger("shootRight3");
                    break;
                case float n when (-150 < n && n < -110):
                    
                    animator.SetTrigger("shootRight2");
                    break;
                case float n when (-180 < n && n < -150):
                    
                    animator.SetTrigger("shootRight1");
                    break;
            }
        }

        if (inputCoin > 0 && (coins > 0 || bulletCasings > 0))
        {
            Vector2 coinVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerToMouseVector = (coinVector - body.position).normalized;
            GameObject newCoin;
            if (bulletCasings > 0)
            {
                newCoin = Instantiate(casingPrefab, body.position + playerToMouseVector, Quaternion.identity);
                bulletCasings--;
                SoundManagerScript.playSound("dropCasing");
                textCasings.text = bulletCasings.ToString();
            }
            else
            {
                newCoin = Instantiate(coinPrefab, body.position + playerToMouseVector, Quaternion.identity);
                coins--;
                SoundManagerScript.playSound("dropCoin");
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
            SoundManagerScript.playSound("pocionDrink");
            metalReserve += 800;
            metalBar.SetValue(metalReserve);
            textVials.text = metalVials.ToString();
            inputVial = 0;
        }

        if (inputPunch > 0)
        {
            Vector2 coinVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerToMouseVector = (coinVector - body.position).normalized / 2;
            float angle = Vector2.SignedAngle(new Vector2(0, 1), playerToMouseVector);
            GameObject newPunch = Instantiate(punchPrefab, body.position + playerToMouseVector, Quaternion.identity);
            newPunch.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(coinVector.y - transform.position.y, coinVector.x - transform.position.x) * Mathf.Rad2Deg);
            newPunch.GetComponent<Rigidbody2D>().AddForce(playerToMouseVector * (currentMass / 3), ForceMode2D.Impulse);
            Destroy(newPunch, 0.085f);
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
            inputPunch = 0;
        }

        if (metalVision)
        {
            metalReserve -= 0.01f;
            metalBar.SetValue(metalReserve);
        }

        if (metalBubble)
        {
            metalReserve -=0.4f;
            metalBar.SetValue(metalReserve);
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
        if (collision.gameObject.tag == "Platform")
        {
            isFalling = false;
            jumped = false;
            animator.SetBool("Falling", false);
        }

        if (collision.gameObject.tag == "Metal" || collision.gameObject.tag == "Aluminum")
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 damageVector = rb.velocity * rb.mass - body.velocity * body.mass;
            float damage = damageVector.magnitude / Random.Range(15, 30);
            if (rb.mass < 1.1f)
            {
                Destroy(collision.gameObject, 0.1f);
            }
            else {
                damage = damage / 10;
            }

            if (damage > 5)
            {
                health -= damage;
                SoundManagerScript.playSound("hurt");
                
                healthBar.SetValue(health);
            }

            
        }

        if (collision.gameObject.tag == "EnemyPunch")
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 damageVector = rb.velocity * rb.mass - body.velocity * body.mass;
            float damage = damageVector.magnitude / Random.Range(5, 30);
            health -= damage;
            SoundManagerScript.playSound("hurt");
            healthBar.SetValue(health);
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