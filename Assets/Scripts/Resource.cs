using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

    public float health = 33f;
    public BarController bar;
    public float decrement = 6f;
    public JaveController jave;
    public GameObject drop;
    public GameObject seed = null;
    public bool chest = false;
    public bool isFirstChest = false;

    bool alive = true;

    bool check = true;

    void Start() {
       jave = GameObject.FindGameObjectWithTag("Player").GetComponent<JaveController>();

        if (chest && isFirstChest && jave.neuralNetworks>0)
        {
            Destroy(gameObject);
        }
    }

	void Update () {
        if (check)
        {
            if (Input.GetMouseButtonDown(0) && alive) {
                Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D hit = Physics2D.OverlapPoint(point);

                if (hit!=null && hit.gameObject == gameObject) {
                    health -= decrement;
                    jave.source.pitch = 1f;
                    if (health <= 0)
                    {
                        check = false;
                        if (seed != null)
                        {
                            Instantiate(seed, transform.position, Quaternion.identity);
                        }
                        jave.source.PlayOneShot(jave.die);
                        Instantiate(drop, transform.position, Quaternion.identity);
                        GetComponentInChildren<ParticleSystem>().Play();
                        GetComponent<SpriteRenderer>().enabled = false;
                        bar.gameObject.SetActive(false);
                        alive = false;
                        Destroy(gameObject, 2f);
                    } else
                    {
                        jave.source.PlayOneShot(jave.hit);
                    }
                }
            }
        }
        float amount = Mathf.Lerp(bar.current, 33f - health, Time.deltaTime * 3f);
        bar.SetAmount(amount);

    }
}
