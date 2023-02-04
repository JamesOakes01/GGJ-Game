using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Defaults")]
    [InspectorName("Spawn Location")]
    public Vector2 spawnLocation = Vector2.zero;
    [InspectorName("Player Speed")]
    public float speed = 0.5f;
    [InspectorName("Run Speed")]
    public float runSpeed = 0.7f;

    [Header("Sprites")]
    public Sprite[] sprites;

    [Header("Game Settings")]
    [InspectorName("Zoom Sensetivity")]
    public float zoomSense = 5;

    private Sprite currentSprite;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.position = spawnLocation;
        currentSprite = sprites[0];
        this.GetComponent<SpriteRenderer>().sprite = currentSprite;
    }

    // Update is called once per frame
    void Update()
    {
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

            Vector2 currenPost = new Vector2(this.transform.position.x, this.transform.position.y);
            this.GetComponent<Rigidbody2D>().MovePosition(currenPost + new Vector2(xAxis*speed, yAxis*speed));
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            // Camera Zoom Magic
            Camera cam = gameObject.GetComponentInChildren<Camera>();
            cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * zoomSense;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 50, 90);
        }

        if(this.GetComponent<SpriteRenderer>().sprite != currentSprite)
            this.GetComponent<SpriteRenderer>().sprite = currentSprite;
    }
}
