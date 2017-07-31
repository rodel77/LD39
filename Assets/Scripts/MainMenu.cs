using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Text sound;
    public Text completed;
    public AudioSource source;

    void Start()
    {
        UpdateUI();
    }

	void UpdateUI () {
        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetInt("Sound", 1);
        }

        source.enabled = PlayerPrefs.GetInt("Sound")==1;

        sound.text = PlayerPrefs.GetInt("Sound")==1 ? "Sound: On" : "Sound: Off";
        completed.text = PlayerPrefs.HasKey("Completed") ? "Completed!" : "Incompleted!";
    }

    public void ToggleSound()
    {
        PlayerPrefs.SetInt("Sound", PlayerPrefs.GetInt("Sound")==1 ? 0 : 1);
        UpdateUI();
    }

    public void Reset()
    {
        PlayerPrefs.SetFloat("Power", 33f);
        PlayerPrefs.SetInt("Logs", 0);
        PlayerPrefs.SetInt("Neural", 0);
    }
	
    public void Play()
    {
        SceneManager.LoadScene("story");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
