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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(timer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator timer()
    {
        Instantiate(Ant);
        yield return new WaitForSeconds(5f);
    }
}
