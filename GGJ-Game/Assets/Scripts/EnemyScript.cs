using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float damageToPlayer = 10;
    float speed;

    private GameObject globalSettings;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("GlobalSettings"))
            globalSettings = GameObject.Find("GlobalSettings");

        Destroy(gameObject, 10f);
        this.gameObject.transform.eulerAngles = new Vector3(
        this.gameObject.transform.eulerAngles.x,
        this.gameObject.transform.eulerAngles.y,
        this.gameObject.transform.eulerAngles.z + rand("rotation")
    );
        speed = rand("speed");
    }

    // Update is called once per frame
    void Update()
    {
        if (globalSettings)
            if ((bool)Variables.Object(globalSettings).Get("IsPaused")) return;

        //this.gameObject.transform.Translate()
        this.gameObject.transform.Translate(Vector3.up * speed *Time.deltaTime);
    }

    float rand(string purpose)
    {
        if (purpose == "rotation")
        {
            return Random.Range(0, 364);
        }
        if (purpose == "speed")
        {
            return Random.Range(minSpeed, maxSpeed);
        }
        else
            return 0;

    }

    public float getDamage()
    {
        return this.damageToPlayer;
    }
}
