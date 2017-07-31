using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour {

	public GameObject box1;
	public GameObject box2;
	public GameObject textPanel;
	public Text text;
	public string message;
    public JaveController jave;

	bool writing = false;
	float last = -1;
	int msgIndex = 0;
	public bool displaying = false;
    bool del = false;

	// Update is called once per frame
	void Update () {
		if (displaying) {
			if (!writing) {
				if (last == -1) {
					last = Time.time;
				}
				
				if (Time.time - last > 0.3f) {
					if (!box1.activeInHierarchy) {
						box1.SetActive (true);
					} else if (!box2.activeInHierarchy) {
						box2.SetActive (true);
					} else {
						textPanel.SetActive (true);
						writing = true;
					}
					last = Time.time;
				}
                jave.talking = false;
            } else {
				text.text = message.Substring (0, msgIndex);
				if (Time.time - last > 0.1f) {
					if (msgIndex < message.Length) {
						msgIndex++;
                        jave.talking = true;
                        float pitch = jave.source.pitch;
                        jave.source.PlayOneShot(jave.bip, 0.5f);
                        jave.source.pitch = Random.Range(0.5f, 2f);
                    } else {
                        if (!del)
                        {
                            StartCoroutine("WaitAndReset");
                        }
                        del = true;
                        jave.talking = false;
                    }
					last = Time.time;
				}
			}
		} else {
			if (box1.activeInHierarchy) {
				reset ();
			}
		}
	}

	void reset(){
        jave.talking = false;
        box1.SetActive (false);
		box2.SetActive (false);
		textPanel.SetActive (false);
		last = -1;
		writing = false;
		msgIndex = 0;
        del = false;
        jave.source.pitch = 1;
    }

    IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(2);
        displaying = false;
        reset();
    }

	public void display(string message){
        if (!displaying)
        {
		    reset ();
            text.text = "";
		    this.message = message;
            displaying = true;
        }
	}
}
