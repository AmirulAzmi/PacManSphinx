using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class commandTest : MonoBehaviour {
    public SphinxWin sphinx;
    public List<Animator> anims;
    public Text micText;
    public Text commandText;
	// Use this for initialization
	void Start () {
        sphinx = GetComponent<SphinxWin>();
        anims = new List<Animator>();
        anims.Add(GameObject.Find("ArrowUp").GetComponent<Animator>());
        anims.Add(GameObject.Find("ArrowDown").GetComponent<Animator>());
        anims.Add(GameObject.Find("ArrowLeft").GetComponent<Animator>());
        anims.Add(GameObject.Find("ArrowRight").GetComponent<Animator>());
        sphinx.m_detected = "none";
	}
	
	// Update is called once per frame
    void Update()
    {
        micText.text = "Mic: " + sphinx.m_mic;
        commandText.text = "Command: " + sphinx.m_detected.ToUpper();

        if (sphinx.m_detected == "up") { Highlight(0); }
        else if (sphinx.m_detected == "down") { Highlight(1); }
        else if (sphinx.m_detected == "left") { Highlight(2); }
        else if (sphinx.m_detected == "right") { Highlight(3); }
        else { StartCoroutine(DimAll()); }
    }

    IEnumerator DimAll() {
        foreach (Animator a in anims) {
            a.SetBool("highlight", false);
        }
        yield return new WaitForSeconds(0);
    }
    void Highlight(int i) {
        StartCoroutine(DimAll());
        anims[i].SetBool("highlight", true);
    }
}
