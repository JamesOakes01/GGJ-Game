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


        //public float randomHorizontal = Random.Range(left, right);
        //public float randomVertical = Random.Range(bottom, top);

        StartCoroutine(timer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator timer()
    {
        //Instantiate(Ant, /*randomHorizontal*/13f,/*randomVertical*/10f,0f);
        Instantiate(Ant, new Vector3(horizontal, vertical, 0f), Quaternion.identity);
        yield return new WaitForSeconds(5f);
    }
}
