using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour {

    public float h;
    public float a;
    public bool rotate;
	
	void Update () {
        if (rotate)
        {
            transform.Rotate(0, 1f, 0f);
        }
        transform.Translate(new Vector3(0, Mathf.Sin(Time.time * a) * h, 0));
	}
}
