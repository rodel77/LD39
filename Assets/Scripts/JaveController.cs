using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class JaveController : MonoBehaviour {

	public Animator jave_sprite;
	public Transform wheel;
	public Transform jave;
    public BarController barController;
    public GameObject selector;
    public ParticleSystem particles;
    public MessageManager messages;
	public float blinkRate = 1.5f;
    public float powerRate = 1.5f;
    public float powerAmount = 0.1f;
	public float wheelSpeed = 1.1f;
	public bool talking = false;
	public bool walking = false;
	public float walkSide = 0;
	public float walkSpeed = 0.3f;
	public float maxRotation = 20f;
	public float resetRotSpeed = 1f;
    public float currentPower = 33f;
    public float snapValue = 1f;
    public float cameraDelta = 4f;
    public bool sad = false;
    public bool happy = false;
    public bool dying = false;
    public bool cameraShe = false;
    public GameObject she;

    public Text neuralText;
    public Text logsText;

    public int logs = 0;
    public int neuralNetworks = 0;

    public AudioClip hit;
    public AudioClip die;
    public AudioClip pickup;
    public AudioClip bip;
    public AudioClip pickup2;

    public AudioSource source;
    public AudioSource source2;

    public GameObject[] tiles;

    public bool inAnimation = false;

    float lastBlink = -1;
    float lastDowngrade = -1;

    float round(float input)
    {
        return snapValue * Mathf.Round(input / snapValue);
    }

    public void SavePower()
    {
        PlayerPrefs.SetFloat("Power", currentPower);
    }

    public void SaveResources()
    {
        PlayerPrefs.SetInt("Logs", logs);
        PlayerPrefs.SetInt("Neural", neuralNetworks);
        UpdateResources();
    }

    public void LoadResources()
    {
        if (PlayerPrefs.HasKey("Logs"))
        {
            logs = PlayerPrefs.GetInt("Logs");
        }

        if (PlayerPrefs.HasKey("Neural"))
        {
            neuralNetworks = PlayerPrefs.GetInt("Neural");
        }

            Debug.Log(PlayerPrefs.GetFloat("Power"));
        if (PlayerPrefs.HasKey("Power"))
        {
            currentPower = PlayerPrefs.GetFloat("Power");
        }
        UpdateResources();
    }

    void Awake()
    {
        source2.enabled = source.enabled = PlayerPrefs.HasKey("Sound") ? PlayerPrefs.GetInt("Sound")==1 : false;
        UpdateResources();
        LoadResources();
        source = GetComponent<AudioSource>();
    }

    public void UpdateResources()
    {
        neuralText.text = ""+neuralNetworks;
        logsText.text = ""+logs;
    }

	void Update () {
        
        if (Input.GetAxis("Cancel")!=0)
        {
            SceneManager.LoadScene("menu");
        }

		jave_sprite.SetBool ("isTalking", talking);
        jave_sprite.SetBool("sad", sad);
        jave_sprite.SetBool("happy", happy);

        Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selector.transform.position = new Vector3(vector3.x, vector3.y, 0);

        float hAxis = Input.GetAxis ("Horizontal");
		float vAxis = Input.GetAxis ("Vertical");

        if (inAnimation)
        {
            hAxis = vAxis = 0;
        }

		if ((walkSide = hAxis) != 0 || vAxis != 0) {
			if (jave.rotation.z*100 <= maxRotation && jave.rotation.z*100 >= maxRotation * -1) {
				jave.RotateAround (wheel.position, Vector3.forward, walkSide * 0.5f * -1f);
			}
			walking = true;
		} else {
			walking = false;
		}

		if (hAxis==0 || !walking) {
		    jave.rotation = Quaternion.Slerp (jave.rotation, new Quaternion(0, 0, 0, 1), resetRotSpeed * Time.deltaTime);
            jave.localPosition = Vector3.Slerp(jave.localPosition, Vector3.zero, resetRotSpeed * Time.deltaTime);
		}

		if (walking) {
			wheel.Rotate(Vector3.forward * wheelSpeed * (walkSide * -1));
		}
        if (!inAnimation)
        {
            Blink();
            Power();
        }
        else
        {
            if (dying)
            {
                sad = true;
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 2, Time.deltaTime * 0.4f);
            }
        }
    }

    IEnumerator WaitAndEnd()
    {
        yield return new WaitForSeconds(5);
        currentPower = 33f;
        logs = 0;
        neuralNetworks = 0;
        SavePower();
        SaveResources();
        SceneManager.LoadScene("menu", LoadSceneMode.Single);
    }

    void Power()
    {
        if (lastDowngrade == -1)
        {
            lastDowngrade = Time.time;
        }

        if(Time.time - lastDowngrade > powerRate)
        {
            currentPower -= powerAmount;
            lastDowngrade = Time.time;
            SavePower();
            if (currentPower <= 0)
            {
                inAnimation = true;
                dying = true;
                StartCoroutine("WaitAndEnd");
            }
        }

        walkSpeed = currentPower / 100f ;

        float amount = Mathf.Lerp(barController.current, 33 - currentPower, Time.deltaTime * 3f);
        barController.SetAmount(amount);
    }

    void Blink()
    {
        if (lastBlink == -1)
        {
            lastBlink = Time.time;
        }

        if (Time.time - lastBlink > blinkRate)
        {
            lastBlink = Time.time;
            if (!talking)
            {
                jave_sprite.SetTrigger("blink");
            }
        }
    }

	void FixedUpdate(){
		if(walking){
            float v = Input.GetAxis ("Vertical");
			GetComponent<Rigidbody2D> ().AddForce(new Vector2(walkSide * walkSpeed, v * walkSpeed), ForceMode2D.Impulse);
		}
        Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, cameraShe ? new Vector3(she.transform.position.x, she.transform.position.y, -1f) : new Vector3(transform.position.x, transform.position.y, -11f), Time.deltaTime * cameraDelta);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Orb") {
            currentPower += 10;
            particles.transform.position = transform.position;
            particles.Play();
            source.PlayOneShot(pickup);
            currentPower = Mathf.Clamp(currentPower, 1, 33);
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Log")
        {
            source.pitch = 1;
            source.PlayOneShot(pickup2);
            Destroy(other.gameObject);
            logs += 1;
            SaveResources();
            UpdateResources();
        }

        if (other.gameObject.tag == "NeuralNetwork")
        {
            source.pitch = 2f;
            source.PlayOneShot(pickup2);
            Destroy(other.gameObject);
            neuralNetworks += 1;
            SaveResources();
            UpdateResources();
        }
    }

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Floor") {
			walking = false;
		}
	}

    public void message(string message)
    {
        messages.display(message);
    }
}
