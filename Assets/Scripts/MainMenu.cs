//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public SphinxWin sphinx;
    public Text micText;

	// Use this for initialization
	void Start () {
        sphinx = GetComponent<SphinxWin>();	
	}
	
	// Update is called once per frame
	void Update () {
        micText.text = sphinx.m_mic;
	}
}