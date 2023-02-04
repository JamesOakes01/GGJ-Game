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
    
    // Players walking speed
    [InspectorName("Walk Speed")]
    public float speed = 0.5f;

    // Players run speed
    [InspectorName("Run Speed")]
    public float runSpeed = 0.7f;

    // Sprite list
    [Header("Sprites")]
    public Sprite[] sprites;

    // (DEFAULT) Zoom Sensitivity
    [Header("Game Settings")]
    [InspectorName("Zoom Sensitivity")]
    public float zoomSense = 5;

    // Minimum Zoom
    [InspectorName("Minimum Zoom")]
    public float minFOV = 50;

    // Maximum Zoom
    [InspectorName("Maximum Zoom")]
    public float maxFOV = 100;

    // (HIDDEN) Player Object
    [HideInInspector]
    public Player player;

    void Start()
    {
        // If GlobalSettings set
        if(GameObject.Find("GlobalSettings"))
        {
            GameObject globalSettings = GameObject.Find("GlobalSettings");
            this.zoomSense = (float)Variables.Object(globalSettings).Get("ZoomSensitivity");
        }

        // Create Player
        player = new Player(this.gameObject, sprites[0]);
    }

    void Update()
    {
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

        // Handle Camera Zooming
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            // Camera Zoom Magic
            Camera cam = gameObject.GetComponentInChildren<Camera>();
            cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * zoomSense;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minFOV, maxFOV);
        }
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
        private Camera cam;
        private SpriteRenderer renderer;

        private Vector2 moveTowards;

        public Player(GameObject parent, Sprite initialSprite)
        {
            this.parent = parent;
            this.body = parent.GetComponent<Rigidbody2D>();
            this.cam = parent.GetComponentInChildren<Camera>();
            this.renderer = parent.GetComponent<SpriteRenderer>();

            if (!parent || !body || !cam) throw new MissingComponentException("Player Not Setup Correctly.");
            if (!initialSprite) throw new MissingComponentException("Player must be provided an initial sprite.");
            renderer.sprite = initialSprite;
        }

        public void move(Vector2 moveTowards)
        {
            if (!checkWriteAccess(System.Environment.StackTrace)) throw new System.Exception("Illegal Access to Player.move()\nOnly A PlayerController may use this.");
            body.MovePosition(moveTowards);
        }

        public void setPosition(Vector2 pos)
        {
            if (!checkWriteAccess(System.Environment.StackTrace)) throw new System.Exception("Illegal Access to Player.setPosition()\nOnly A PlayerController may use this.");
            parent.transform.position = pos;
        }

        public void setSprite(Sprite sprite)
        {
            if (!checkWriteAccess(System.Environment.StackTrace)) throw new System.Exception("Illegal Access to Player.setSprite()\nOnly A PlayerController may use this.");
            parent.GetComponent<SpriteRenderer>().sprite = sprite;
        }

        public Sprite getSprite()
        {
            return this.renderer.sprite;
        }

        public Vector2 getPosition()
        {
            return this.parent.transform.position;
        }

        /*
         * Prevent Anything other than authorized controllers from setting things.
         */
        private bool checkWriteAccess(string stackTrace)
        {
            if (stackTrace.Split("\n")[2].Contains("PlayerController.Update () "))
            {
                //return true;
            }

            return false;
        }

    }
}
