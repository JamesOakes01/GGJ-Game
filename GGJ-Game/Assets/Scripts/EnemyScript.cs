using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 10f);
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
            Debug.Log("ran");
            return Random.Range(minSpeed, maxSpeed);
        }
        else
            return 0;

    }
}
