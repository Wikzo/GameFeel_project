using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StateManager : MonoBehaviour
{
    public bool UseRandomValuesAtStart = true;

    public Player MyPlayer;

    private PostDataOnline _myPostDataOnline;
    private ParameterManager _paramateManager;

    public AudioClip GameMusic;
    public AudioClip QuestionnaireMusic;
    private AudioSource _audioSource;

    public List<Star> StarsSegment1;
    public List<Star> StarsSegment2;
    public List<Star> StarsSegment3;
    public List<GameObject> StarsIcons;

    public List<Checkpoint> Checkpoints;

    public int CollectedSoFar = 0;
    public int CollectToWin = 3;

    public float TimeSpentOnLevel;
    public int DeathsOnThisLevel;

    void Start()
    {
        _myPostDataOnline = GetComponent<PostDataOnline>();
        _audioSource = GetComponent<AudioSource>();

        foreach (GameObject c in ParameterManager.Instance.MyQuestionnaireUI)
            c.SetActive(false);

        if (UseRandomValuesAtStart)
            MyPlayer.ChangeParameters();

        Restart();

        StartCoroutine(StartMusicAfterDelay());
    }

    IEnumerator StartMusicAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeMusicIn(GameMusic));
    }
    

    IEnumerator FadeMusicIn(AudioClip clip)
    {
        float t = 0;

        _audioSource.clip = clip;
        _audioSource.Play();
        _audioSource.loop = true;

        while (t < 1)
        {
            
            t += Time.deltaTime;
            audio.volume = t;

            yield return null;
        }
    }

    IEnumerator FadeMusicOut(AudioClip clip)
    {
        float t = 1;

        while (t > 0)
        {
            
            t -= Time.deltaTime*2;
            audio.volume = t;

            yield return null;
        }

        StartCoroutine(FadeMusicIn(clip));

    }

    public void ContinueToNextRound()
    {
        _myPostDataOnline.PostData(Demographics.Instance.YourName,
            Demographics.Instance.ToStringDatabaseFormat(),
            ParameterManager.Instance.MyQuestionnaireData[ParameterManager.Instance.Index].QuestionnaireDataToDatabaseFormat(),
            MyPlayer.MyTweakableParameters.ToStringDatabaseFormat(),
            StateManager.Instance.AverageFps);

        Demographics.Instance.MyGameState = GameState.Playing;

        StartCoroutine(FadeMusicOut(GameMusic));



        if (ParameterManager.Instance.Index + 1 < ParameterManager.Instance.MyParameters.Count)
            ParameterManager.Instance.Index++;
        else
            ParameterManager.Instance.Index = 0;

        foreach (GameObject c in ParameterManager.Instance.MyQuestionnaireUI)
            c.SetActive(false);

        MyPlayer.ChangeParameters();
        MyPlayer.Restart();

        Restart();
    }

    public void Restart()
    {
        TimeSpentOnLevel = 0;
        DeathsOnThisLevel = 0;
        totalTime = 0;
        totalFrames = 0;


        Time.timeScale = 1;

        foreach (Star s in StarsSegment1)
        {
            s.gameObject.SetActive(true);
            s.Restart();
            s.gameObject.SetActive(false);
        }

        foreach (Star s in StarsSegment2)
        {
            s.gameObject.SetActive(true);
            s.Restart();
            s.gameObject.SetActive(false);
        }

        foreach (Star s in StarsSegment3)
        {
            s.gameObject.SetActive(true);
            s.Restart();
            s.gameObject.SetActive(false);
        }

        int random1 = Random.Range(0, StarsSegment1.Count);
        int random2 = Random.Range(0, StarsSegment1.Count);
        int random3 = Random.Range(0, StarsSegment1.Count);

        StarsSegment1[random1].gameObject.SetActive(true);
        StarsSegment2[random2].gameObject.SetActive(true);
        StarsSegment3[random3].gameObject.SetActive(true);

        StarsSegment1[random1].Restart();
        StarsSegment2[random2].Restart();
        StarsSegment3[random3].Restart();

        CollectedSoFar = 0;

        foreach (GameObject g in StarsIcons)
            g.transform.renderer.enabled = true;

        foreach (Checkpoint c in Checkpoints)
            c.Restart();

    }

    void Update()
    {
        if (Demographics.Instance.MyGameState != GameState.MidQuestionnaire)
        {
            Time.timeScale = 1;

            CountAverageFPS();
            TimeSpentOnLevel += Time.deltaTime;
        }


        if (CollectedSoFar > CollectToWin - 1 && Demographics.Instance.MyGameState == GameState.Playing)
            StartCoroutine(ShowQuestionnaire());


    }

    IEnumerator ShowQuestionnaire()
    {
        yield return new WaitForSeconds(0.3f);
        Debug.Log("Game won!");

        ParameterManager.Instance.MyQuestionnaireUI[ParameterManager.Instance.Index].SetActive(true);

        Demographics.Instance.MyGameState = GameState.MidQuestionnaire;

        StartCoroutine(FadeMusicOut(QuestionnaireMusic));

    }

    public string AverageFps
    {
        get { return string.Format("&FPS={0}", (totalTime / totalFrames).ToString("0.00")); }
    }

    private float totalTime;
    private float totalFrames;

    public void CountAverageFPS()
    {
        totalTime += Time.timeScale / Time.deltaTime;
        ++totalFrames;
    }

    private static StateManager _instance;
    public static StateManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType(typeof(StateManager)) as StateManager;

            return _instance;
        }
    }

    void OnApplicationQuit()
    {
        _instance = null; // release on exit
    }

    private void Awake()
    {
        // http://clearcutgames.net/home/?p=437
        // First we check if there are any other instances conflicting
        if (_instance != null && _instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }

        // Here we save our singleton instance
        _instance = this;

        // Furthermore we make sure that we don't destroy between scenes (this is optional)
        DontDestroyOnLoad(gameObject);

    }
}
