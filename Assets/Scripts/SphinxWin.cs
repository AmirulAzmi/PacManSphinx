using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pocketsphinx;

public class SphinxWin : MonoBehaviour {
    private PsDecoder _decoder;

    public string m_mic;
    public string m_detected;
    public int m_micIndex = 0;
    public string m_grammar;

    private bool _listening;
    private bool _processing;

    private float transition = 0.0f;
    //private float d = 4.0f;

    private List<string> micOption = new List<string>();
    private AudioSource aud;
    public void Awake()
    {
        var config = new CmdLineConfig();
        config.Hmm = Application.dataPath + "\\StreamingAssets\\SphinxAssets\\Models\\en-us\\en-us";
        config.Dict = Application.dataPath + "\\StreamingAssets\\SphinxAssets\\Models\\en-us\\cmudict-en-us.dict";
        config.Logfn = Application.dataPath + "\\StreamingAssets\\SphinxAssets\\sphinxlog.txt";
        config.Jsgf = Application.dataPath + "\\StreamingAssets\\SphinxAssets\\" + m_grammar;

        aud = GetComponent<AudioSource>();
        _decoder = new PsDecoder(config);

        _listening = true;
        _processing = false;

        foreach (string device in Microphone.devices)
        {
            micOption.Add(device);
        }
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
    void Update()
    {
        if (transition > 1.0f)
        {
            if (_listening) { StartUtterance(); }
            if (_processing) { EndUtterance(); }
        }
        else {
            transition += Time.deltaTime; // d;
        }
    }

    IEnumerator CaptureUtterance() 
    {
        aud.clip = Microphone.Start(m_mic, true, 3, 16000);

        int readHead = 0; // position in the clip we're reading from
        int writeHead;    // position in the clip last written too by mic
        int nFloatsToGet; // number of new samples available in the clip
        float[] buffer;   // buffer to copy clip data into

        while (true) {
            writeHead = Microphone.GetPosition(m_mic);
            nFloatsToGet = (aud.clip.samples + writeHead - readHead) % aud.clip.samples;
            if (nFloatsToGet < 1024) {
                yield return null;
                continue;
            }

            buffer = new float[nFloatsToGet];

            aud.clip.GetData(buffer, readHead);
            readHead = (readHead + nFloatsToGet) % aud.clip.samples;
            _decoder.ProcessRaw(buffer, 0, nFloatsToGet);

            float score;
            string uttid;
            string hypothesis = _decoder.GetHypothesis(out score, out uttid);
            if (hypothesis != null)
            {
                Debug.Log(hypothesis);
                _listening = false;
                _processing = true;
                yield break;
            }
            yield return null;
        }
    }

    void StartUtterance() 
    {
        _listening = false;

        _decoder.StartUtt(System.Guid.NewGuid().ToString());
        m_mic = Microphone.devices[m_micIndex];

        StartCoroutine(CaptureUtterance());
    }

    void EndUtterance() 
    {
        _processing = false;
        
        StopAllCoroutines();
        Microphone.End(m_mic);
        _decoder.EndUtt();

        float score;
        string uttid;
        string hypothesis = _decoder.GetHypothesis(out score, out uttid);
        m_detected = hypothesis;
        StartCoroutine(Wait());

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        _listening = true;
    }
}
