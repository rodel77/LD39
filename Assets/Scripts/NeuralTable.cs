using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NeuralTable : MonoBehaviour {

    public JaveController jave;
    public GameObject she;
    public GameObject sheWheel;
    public GameObject shePoint;

    bool end = false;
    bool en = true;

	void Update () {
        if (Input.GetMouseButton(0) && en)
        {
            Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(point);

            if (hit != null && hit.gameObject == gameObject) {
                if (jave.neuralNetworks>=7)
                {
                    en = false;
                    jave.currentPower = 33f;
                    jave.inAnimation = true;
                    jave.cameraShe = true;
                    jave.logs = 0;
                    jave.neuralNetworks = 0;
                    jave.SavePower();
                    jave.SaveResources();
                    PlayerPrefs.SetInt("Completed", 1);
                    StartCoroutine("EndS");
                    StartCoroutine("End2S");
                    StartCoroutine("End3S");
                    StartCoroutine("End4S");
                }
            }
        }

        if (!en)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 2, Time.deltaTime * 2f);
        }

        if (end)
        {
            sheWheel.transform.Rotate(Vector3.forward * 4f);
            she.transform.position = Vector3.Slerp(she.transform.position, shePoint.transform.position, Time.deltaTime * 1f);
        }
    }

    IEnumerator EndS()
    {
        yield return new WaitForSeconds(4f);
        end = true;
    }

    IEnumerator End2S()
    {
        yield return new WaitForSeconds(6f);
        jave.cameraShe = false;
    }

    IEnumerator End3S()
    {
        yield return new WaitForSeconds(9f);
        end = false;
        jave.happy = true;
    }

    IEnumerator End4S()
    {
        yield return new WaitForSeconds(15f);
        SceneManager.LoadScene("end");
    }
}
