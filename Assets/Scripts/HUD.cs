//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    private GameManager gm; 
    public SphinxWin sphinx;

    public Image inputButton;
    public Sprite inputMicImage;
    public Sprite inputSwipeImage;
    public Text score;
    public Text life;
    public Text info;
    public Text hiscore;
    public Text command;
    public Text logtext;
    public Text usespeech;
    public Text currentLevel;
    private bool isPaused = false;
    public GameObject pauseHud;
    public GameObject pauseBtn;

    void Start()
    {
        gm = GameObject.Find("Managers").GetComponent<GameManager>();
        sphinx = GameObject.Find("sphinx").GetComponent<SphinxWin>();
        int lvl = PlayerPrefs.GetInt("level");
        if (lvl == 0) {
            lvl = 1;
        }
        
        currentLevel.text = "Level: " + lvl.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        score.text = "Score: " + gm.m_score.ToString();
        life.text = "Life: " + gm.m_life.ToString();
        info.text = "Info: " + sphinx.m_mic;
        hiscore.text = "Highscore: " + gm.m_hiscore.ToString();
        if (gm.useSpeech)
        {
            command.text = "Command: " + sphinx.m_detected.ToUpper();
            usespeech.text = "using microphone";
            inputButton.sprite = inputMicImage;
        }
        else
        {
            command.text = "Command: - ";
            usespeech.text = "using keyboard";
            inputButton.sprite = inputSwipeImage;
            info.text = "Info: Use arrow keys to control Pac-man";
        }
	}

    public void TogglePauseGame() {
        if (!isPaused)
        {
            isPaused = true;
            gm.PauseGame();
            //Time.timeScale = 0;
            pauseHud.SetActive(true);
            pauseBtn.SetActive(false);
        }
        else {
            isPaused = false;
            gm.ResumeGame();
            //Time.timeScale = 1;
            pauseHud.SetActive(false);
            pauseBtn.SetActive(true);
        }
    }
    public void ToggleSpeech()
    {
        if (!gm.useSpeech) { gm.useSpeech = true; }
        else { gm.useSpeech = false; }
    }

    public void ExitGame() {
        //Time.timeScale = 1;
        StartCoroutine(gm.ToScoreView());
    }
}
