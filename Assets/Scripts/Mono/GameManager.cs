using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviour {
    PhotonView photonView;
    #region Variables

    private             Question[]          _questions              = null;
    public              Question[]          Questions               { get { return _questions; } }

    [SerializeField]    GameEvents          events                  = null;

    [SerializeField]    Animator            timerAnimtor            = null;
    [SerializeField]    TextMeshProUGUI     timerText               = null;
    [SerializeField]    Color               timerHalfWayOutColor    = Color.yellow;
    [SerializeField]    Color               timerAlmostOutColor     = Color.red;
    private             Color               timerDefaultColor       = Color.black;

    private             List<AnswerData>    PickedAnswers           = new List<AnswerData>();
    private             List<int>           FinishedQuestions       = new List<int>();
    private             int                 currentQuestion         = 0;

    private             int                 timerStateParaHash      = 0;

    private             IEnumerator         IE_WaitTillNextRound    = null;
    private             IEnumerator         IE_StartTimer           = null;
    private             bool                IsFinished
    {
        get
        {
            return (FinishedQuestions.Count < Questions.Length) ? false : true;
        }
    }


    [SerializeField] public enum QuestionCategory { Type1, Type2, Type3, Type4 };
    public QuestionCategory chooseCategory;
    [SerializeField] Image TimerImageHolder;
    [SerializeField] TextMeshProUGUI QuestionTotalText;

    #endregion

    #region Default Unity methods

    /// <summary>
    /// Function that is called when the object becomes enabled and active
    /// </summary>
    void OnEnable()
    {
        events.UpdateQuestionAnswer += UpdateAnswers;

       StartCoroutine(_DelayDispaly());

    }
    IEnumerator _DelayDispaly()
    {
        yield return new WaitForSeconds(.4f);
        if (PhotonNetwork.IsMasterClient) StatusManger.instance.QuestionIndex = GetRandomQuestionIndex();

        photonView.RPC("Display", RpcTarget.All);
        Accept();
    }
    /// <summary>
    /// Function that is called when the behaviour becomes disabled
    /// </summary>
    void OnDisable()
    {
        events.UpdateQuestionAnswer -= UpdateAnswers;
    }

    /// <summary>
    /// Function that is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    /// </summary>
    void Awake()
    {
        events.CurrentFinalScore = 0;
    }
    /// <summary>
    /// Function that is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        //  events.StartupHighscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        photonView = PhotonView.Get(this);

        timerDefaultColor = timerText.color;

        if (chooseCategory== QuestionCategory.Type1)
        {
            LoadQuestions("Questions1");
        }
        else if (chooseCategory == QuestionCategory.Type2)
        {
            LoadQuestions("Questions2");
        }
        else if (chooseCategory == QuestionCategory.Type3)
        {
            LoadQuestions("Questions3");
        } else if (chooseCategory == QuestionCategory.Type4)
        {
            LoadQuestions("Questions4");
        }

        timerStateParaHash = Animator.StringToHash("TimerState");

        var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        UnityEngine.Random.InitState(seed);

        // Display();
       
    }

    #endregion

    /// <summary>
    /// Function that is called to update new selected answer.
    /// </summary>
    public void UpdateAnswers(AnswerData newAnswer)
    {
        if (Questions[currentQuestion].GetAnswerType == Question.AnswerType.Single)
        {
            foreach (var answer in PickedAnswers)
            {
                if (answer != newAnswer)
                {
                    answer.Reset();
                }
            }
            PickedAnswers.Clear();
            PickedAnswers.Add(newAnswer);
        }
        else
        {
            bool alreadyPicked = PickedAnswers.Exists(x => x == newAnswer);
            if (alreadyPicked)
            {
                PickedAnswers.Remove(newAnswer);
            }
            else
            {
                PickedAnswers.Add(newAnswer);
            }
        }
    }

    /// <summary>
    /// Function that is called to clear PickedAnswers list.
    /// </summary>
    public void EraseAnswers()
    {
        PickedAnswers = new List<AnswerData>();
    }

    /// <summary>
    /// Function that is called to display new question.
    /// </summary>
    [PunRPC]
   public void Display()
   {
        QuestionTotalText.text = StatusManger.instance.QuestionTotal.ToString();
      if(StatusManger.instance.QuestionTotal<=3)  StartCoroutine(_DisplayQuestion());
   }
    IEnumerator _DisplayQuestion()
    {
        yield return new WaitForSeconds(.5f);
        EraseAnswers();
       
            var question = GetRandomQuestion();
            StatusManger.instance.QuestionText.text = question.Info;
            if (events.UpdateQuestionUI != null)
            {
                events.UpdateQuestionUI(question);
            }
            else { Debug.LogWarning("Ups! Something went wrong while trying to display new Question UI Data. GameEvents.UpdateQuestionUI is null. Issue occured in GameManager.Display() method."); }

            if (question.UseTimer)
            {
                UpdateTimer(question.UseTimer);
            }
        

    }
    /// <summary>
    /// Function that is called to accept picked answers and check/display the result.
    /// </summary>
    public void Accept()
    {
        UpdateTimer(false);
        bool isCorrect = CheckAnswers();
        FinishedQuestions.Add(currentQuestion);

        UpdateScore((isCorrect) ? Questions[currentQuestion].AddScore : 0);// -Questions[currentQuestion].AddScore);

        //if (IsFinished)
        //{
        //    SetHighscore();
        //}

        //var type 
        //    = (IsFinished) 
        //    ? UIManager.ResolutionScreenType.Finish 
        //    : (isCorrect) ? UIManager.ResolutionScreenType.Correct 
        //    : UIManager.ResolutionScreenType.Incorrect;

        //if (events.DisplayResolutionScreen != null)
        //{
        //    events.DisplayResolutionScreen(type, Questions[currentQuestion].AddScore);
        //}

      //  AudioManager.Instance.PlaySound((isCorrect) ? "CorrectSFX" : "IncorrectSFX");

        //if (type != UIManager.ResolutionScreenType.Finish)
        //{
        //    if (IE_WaitTillNextRound != null)
        //    {
        //        StopCoroutine(IE_WaitTillNextRound);
        //    }
        //    IE_WaitTillNextRound = WaitTillNextRound();
        //    StartCoroutine(IE_WaitTillNextRound);

        //    Debug.Log("IsFinished");
        //}
    }

    #region Timer Methods

    void UpdateTimer(bool state)
    {
        switch (state)
        {
            case true:
                if (!PhotonNetwork.IsMasterClient)
                {
                    IE_StartTimer = StartTimer();
                    StartCoroutine(IE_StartTimer);
                }
                
                break;
            case false:
                if (IE_StartTimer != null)
                {
                    StopCoroutine(IE_StartTimer);
                }

               // timerAnimtor.SetInteger(timerStateParaHash, 1);
                break;
        }
    }
   // [PunRPC]
    public void TimerGrp()
    {
       
        IE_StartTimer = StartTimer();
        StartCoroutine(IE_StartTimer);
    }
    int timeLeft;
   void Update()
    {
        timeLeft = (int)StatusManger.instance.Timer;// totalTime;

    }
    IEnumerator StartTimer()
    {
       //  var totalTime = Questions[currentQuestion].Timer;

        timerText.color = timerDefaultColor;
        while (timeLeft > 0)
        {
           // timeLeft--;

         
           

            TimerImageHolder.fillAmount = (timeLeft / 45f);
          
            if (timeLeft<30 && timeLeft > 15)
            {
                TimerImageHolder.color = timerHalfWayOutColor;
               
            } else if (timeLeft<15)
            {
                TimerImageHolder.color = timerAlmostOutColor;
                
            }

            timerText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        //Accept();
    }
    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
      //  Display();
    }

    #endregion

    /// <summary>
    /// Function that is called to check currently picked answers and return the result.
    /// </summary>
    bool CheckAnswers()
    {
        if (!CompareAnswers())
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// Function that is called to compare picked answers with question correct answers.
    /// </summary>
    bool CompareAnswers()
    {
        if (PickedAnswers.Count > 0)
        {
            List<int> c = Questions[currentQuestion].GetCorrectAnswers();
            List<int> p = PickedAnswers.Select(x => x.AnswerIndex).ToList();

            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();

            return !f.Any() && !s.Any();
        }
        return false;
    }

    /// <summary>
    /// Function that is called to load all questions from the Resource folder.
    /// </summary>
    void LoadQuestions(string Question_type)
    {
        Object[] objs = Resources.LoadAll(Question_type, typeof(Question));
        _questions = new Question[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            _questions[i] = (Question)objs[i];
        }
    }

    /// <summary>
    /// Function that is called restart the game.
    /// </summary>
    public void RestartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /// <summary>
    /// Function that is called to quit the application.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Function that is called to set new highscore if game score is higher.
    /// </summary>
    private void SetHighscore()
    {
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        if (highscore < events.CurrentFinalScore)
        {
            PlayerPrefs.SetInt(GameUtility.SavePrefKey, events.CurrentFinalScore);
        }
    }
    /// <summary>
    /// Function that is called update the score and update the UI.
    /// </summary>
    private void UpdateScore(int add)
    {
        events.CurrentFinalScore += add;

        if (events.ScoreUpdated != null)
        {
            events.ScoreUpdated();
        }
    }

    #region Getters

    Question GetRandomQuestion()
    {
       
       
       // var randomIndex = StatusManger.instance.QuestionIndex;// GetRandomQuestionIndex();
        currentQuestion = StatusManger.instance.QuestionIndex;
        Debug.Log(StatusManger.instance.QuestionIndex);
        return Questions[currentQuestion];
    }
    int random = -1;
    int GetRandomQuestionIndex()
    {
       
        random++;
        //if (FinishedQuestions.Count < Questions.Length)
        //{
        //    do
        //    {
        //        random = UnityEngine.Random.Range(0, Questions.Length);
        //    } while (FinishedQuestions.Contains(random) || random == currentQuestion);
        //}
        return random;
       
    }

    #endregion
}