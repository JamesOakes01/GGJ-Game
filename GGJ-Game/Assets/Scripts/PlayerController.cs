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
        player = new Player(this.gameObject, sprites[0], globalSettings);
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

        
        player.checkIsDead();
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

        private double health = 100;

        private Vector2 moveTowards;

        public Player(GameObject parent, Sprite initialSprite, GameObject globalSettings)
        {
            this.parent = parent;
            this.body = parent.GetComponent<Rigidbody2D>();
            this.renderer = parent.GetComponent<SpriteRenderer>();
            this.globalSettings = globalSettings;

            if (!parent || !body) throw new MissingComponentException("Player Not Setup Correctly.");
            if (!initialSprite) throw new MissingComponentException("Player must be provided an initial sprite.");
            renderer.sprite = initialSprite;
        }

        public void checkIsDead()
        {
            if (globalSettings == null)
                globalSettings = GameObject.Find("GlobalSettings");

            if (this.health <= 0)
            {
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