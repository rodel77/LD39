﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour {

	public void GotoMenu()
    {
        SceneManager.LoadScene("menu");
    }
}
