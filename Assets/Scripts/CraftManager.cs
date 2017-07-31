using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftManager : MonoBehaviour {

    public RectTransform craftWindow;
    public Text text;
    public JaveController jave;
    public GameObject orb;
    public bool show = false;

    public string str = "Craft Energy: \n5 Wood = +10 Energy";
    public string strNo = "<color=red>You don't have wood to craft!</color>";

    public void OpenCraft()
    {
        show = true;
        UpdateCraft();
    }

    public void UpdateCraft()
    {
        text.text = str;
        if (jave.logs < 5)
        {
            text.text += "\n" + strNo;
        }
    }

    public void Craft()
    {
        if (jave.logs>=5)
        {
            Debug.Log(jave.logs);
            jave.logs -= 5;
            Debug.Log(jave.logs);
            Instantiate(orb, new Vector3(jave.transform.position.x, jave.transform.position.y, jave.transform.position.z), Quaternion.identity);
            jave.SaveResources();
        }
        UpdateCraft();
    }

    public void CloseCraft()
    {
        show = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!show)
        {
            craftWindow.localPosition = Vector3.Slerp(craftWindow.localPosition, new Vector3(0, 600, 0), Time.deltaTime * 7f);
        }
        else
        {
            craftWindow.localPosition = Vector3.Slerp(craftWindow.localPosition, Vector3.zero, Time.deltaTime * 7f);
        }
	}
}
