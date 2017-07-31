using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour {

    public RectTransform grayPanel;
    public RectTransform bluePanel;
    [Range(1, 34)]
    public float current = 30f;

    public void SetAmount(float curr) {
        current = Mathf.Clamp(curr, 1, 34);
    }

	// Update is called once per frame
	void Update () {
        bluePanel.offsetMin = new Vector2(current, 0);//left-bottom
	}
}
