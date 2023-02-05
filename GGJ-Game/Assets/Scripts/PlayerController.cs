using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    [Header("Player Defaults")]

    // Spawn Location
    [InspectorName("Spawn Location")]
    public Vector2 spawnLocation = Vector2.zero;

    [InspectorName("Health Display")]
    public GameObject healthMask;

    [InspectorName("Pause Panel")]
    public GameObject PauseUI;

    // Players walking speed
    [InspectorName("Walk Speed")]
    public float speed = 0.5f;

    // Players run speed
    [InspectorName("Run Speed")]
    public float runSpeed = 0.7f;

    // Sprite list
    [Header("Sprites")]
    public Sprite[] sprites;

    // (HIDDEN) Player Object
    [HideInInspector]
    public Player player;

    [Header("Screens")]

    // Player Died Screen
    [InspectorName("Died Panel")]
    public GameObject diedScreen;


    private GameObject globalSettings;
    
    void Start()
    {
        if (GameObject.Find("GlobalSettings"))
            globalSettings = GameObject.Find("GlobalSettings");

        // Create Player
        player = new Player(this.gameObject, sprites, globalSettings);
    }

    void Update()
    {
        if (globalSettings == null)
            globalSettings = GameObject.Find("GlobalSettings");
        if (globalSettings)
            if ((bool)Variables.Object(globalSettings).Get("IsPaused")) return;

        // Handle Input
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            float xAxis = Input.GetAxis("Horizontal");
            float yAxis = Input.GetAxis("Vertical");

            if (Input.GetButton("Run"))
            {
                xAxis *= runSpeed;
                yAxis *= runSpeed;
            }
            else
            {
                xAxis *= speed;
                yAxis *= speed;
            }

            player.move(new Vector2(this.transform.position.x + xAxis, this.transform.position.y + yAxis));
        }


        Vector3 normalMaskPosition = new Vector3(454, -188, 0);
        Vector3 wantedMaskPosition = new Vector3(454, -88, 0);
        Vector3 normalLeafPositon = new Vector3(0, 0, 0);
        Vector3 wantedLeafPosition = new Vector3(0, -100, 0);
        GameObject.Find("Health").transform.localPosition = Vector3.Lerp(normalMaskPosition, wantedMaskPosition, (float)(player.getHealth() / 100));
        GameObject.Find("HealthLeaf").transform.localPosition = Vector3.Lerp(normalLeafPositon, wantedLeafPosition, (float)(player.getHealth() / 100));
        GameObject.Find("HealthLeaf").GetComponent<UnityEngine.UI.RawImage>().color = Color.Lerp(new Color(.79f, .53f, .35f), new Color(.4f, .8f, .358f), (float)(player.getHealth() / 100));

        if (player.getSprite() == sprites[1])
            StartCoroutine(hitAnim());
        
        player.checkIsDead();
    }


    private bool debounce = false;
    protected IEnumerator hitAnim()
    {
        if (debounce)
            yield return null;

        debounce = true;
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();

        renderer.enabled = false;
        yield return new WaitForSeconds(0.2f);
        renderer.enabled = true;
        yield return new WaitForSeconds(0.2f);
        renderer.enabled = false;
        yield return new WaitForSeconds(0.2f);
        renderer.enabled = true;
        yield return new WaitForSeconds(0.2f);
        renderer.enabled = false;
        yield return new WaitForSeconds(0.2f);
        renderer.enabled = true;
        yield return new WaitForSeconds(0.5f);

        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
        debounce = false;
    }



    /*
     * 
     * Player Class
     * 
     * I don't know why I did this, but I did
     * It works amazingly and lets me access the player without cluttering the rest of the code.
     * 
     */
    public class Player
    {
        private GameObject parent;
        private Rigidbody2D body;
        private SpriteRenderer renderer;
        private GameObject globalSettings;
        private Sprite[] spriteArray;

        private double health = 100;

        private Vector2 moveTowards;

        public Player(GameObject parent, Sprite[] sprites, GameObject globalSettings)
        {
            this.parent = parent;
            this.body = parent.GetComponent<Rigidbody2D>();
            this.renderer = parent.GetComponent<SpriteRenderer>();
            this.globalSettings = globalSettings;
            spriteArray = sprites;

            if (!parent || !body) throw new MissingComponentException("Player Not Setup Correctly.");
            if (sprites == null) throw new MissingComponentException("Player must be provided an initial sprite.");
            renderer.sprite = sprites[0];
        }

        public void checkIsDead()
        {
            if (globalSettings == null)
                globalSettings = GameObject.Find("GlobalSettings");

            if (this.health <= 0)
            {
                parent.GetComponent<SpriteRenderer>().sprite = spriteArray[2];
                parent.GetComponent<PlayerController>().diedScreen.SetActive(true);
                Variables.Object(globalSettings).Set("IsPaused", true);
            }
        }

        public void move(Vector2 moveTowards)
        {
            body.MovePosition(moveTowards);
        }

        public void setPosition(Vector2 pos)
        {
            parent.transform.position = pos;
        }

        public void setSprite(Sprite sprite)
        {
            parent.GetComponent<SpriteRenderer>().sprite = sprite;
        }

        public void damage(float dmg)
        {
            this.health -= dmg;

            checkIsDead();

            if(this.getSprite() != spriteArray[2])
                parent.GetComponent<SpriteRenderer>().sprite = spriteArray[1];
        }

        public void setHealth(double toSet)
        {
            this.health = toSet;

            checkIsDead();
        }

        public double getHealth()
        {
            return this.health;
        }


        public Sprite getSprite()
        {
            return this.renderer.sprite;
        }

        public Vector2 getPosition()
        {
            return this.parent.transform.position;
        }

        
    }
}