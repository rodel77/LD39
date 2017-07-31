using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inspectable : MonoBehaviour {

    public JaveController jave;
    public GameObject text;
    public string[] messages;

    void Start()
    {
        text = GameObject.FindGameObjectWithTag("Selector");
        jave = GameObject.FindGameObjectWithTag("Player").GetComponent<JaveController>();
    }

    void Update()
    {
        Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(point);

        if (hit != null)
        {
            if(hit.gameObject == gameObject)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    jave.message(messages[Random.Range(0, messages.GetLength(0))]);
                }
            }

            if (hit.gameObject.GetComponent<Inspectable>() != null)
            {
                if (text != null && text.gameObject != null)
                {
                    text.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if(text!=null && text.gameObject != null)
            {
                text.gameObject.SetActive(false);
            }
        }
    }
}
