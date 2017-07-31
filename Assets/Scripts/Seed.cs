using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour {

    public GameObject tree;
    public float growTime = 10f;

    private float startGrow = -1f;

	void Start () {
        growTime = Random.Range(15f, 20f);
	}
	
	void Update () {
        if (startGrow == -1)
        {
            startGrow = Time.time;
        }

        if(Time.time - startGrow > growTime)
        {
            Instantiate(tree, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
	}
}
