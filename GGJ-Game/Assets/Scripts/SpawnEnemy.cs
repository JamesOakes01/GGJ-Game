using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] GameObject Ant;

    [Tooltip("Set each value to the corresponding transform value on the OUTER edge of the boarder")]
    [Header("World Boundries")]
    [SerializeField] float left;
    [SerializeField] float right;
    [SerializeField] float top;
    [SerializeField] float bottom;

    [Header("Temp position")]
    [SerializeField] float horizontal;
    [SerializeField] float vertical;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(timer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float rand(string orientation)
    {
        if (orientation == "vertical")
        {
            return Random.Range(bottom, top);
        }
        if (orientation == "horizontal")
        {
            return Random.Range(bottom, top);
        }
        else
            return 0;
        
    }

    IEnumerator timer()
    {
        while (true)
        {
            Instantiate(Ant, new Vector3(rand("horizontal"), rand("vertical"), 0f), Quaternion.identity);
            yield return new WaitForSeconds(5f);
        }
    }
}
