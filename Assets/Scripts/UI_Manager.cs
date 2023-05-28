using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
 
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

 public class UI_Manager : MonoBehaviour
 {
    public static UI_Manager instance;
    public Joystick joyStick;
    public float myScore=0;
    public int currentModeIndex = 0;
    public int currentSelectedSlitherIndex=0;
    public int myRank;

    public Slider timeSliderValue;
    
    [Header(("InputField"))]
    public InputField usernameText;
    public InputField yourAmountInputField;
    public InputField yourAmountInputFieldAI;
    public InputField addFundPopUpInputField;

    [Header(("UI Buttons"))]
    public Button createRoomButton;
    public Button addFundsPopUpButton;
    public Button sprintButton;
    public Button playNowButton;
    public Button backButton;
    public Button settingButton;
   
    
    [Header("Lists & Dictionary")]
    public List<UIPanels> panelInfo= new List<UIPanels>();
    public Dictionary<string, float> scores = new Dictionary<string, float>();
    public List<Text> scoreTexts = new List<Text>();
    public List<Sprite> orbSprites = new List<Sprite>();
    public List<Sprite> chooseSlither = new List<Sprite>();
    public List<Button> selectedModeButtons = new List<Button>();
    public List<Text> selectedModeText = new List<Text>();
    
    
    [Header("Snake body and skin")]
    public List<Sprite> snakeHeadSkin = new List<Sprite>();
    public List<Sprite> snakeBodySkin = new List<Sprite>();
    public List<Sprite> snakeGlowSkin = new List<Sprite>();


    [Header("Screen")]
    public GameObject screens;
    
    
    [Header("Text")]
    public Text convertedAmountText;
    public Text convertedText;
    public Text balanceText;
    public Text searchingRoomText;
    public Text timerText;
    public Text enterNameError;
    public Text enterNameErrorAI;
    public Text enterBetErrorAI;
    public Text roomNameText;
    public Text myscoreText;
    public Text fpsText;
    public Text killsText;
    public Text yourLengthText;
    public Text yourrankText;
    
    
    
    [Header("Stats Text")]
    public Text survivalTimeText;
    public Text playerKilledText;
    public Text tokenEaten;
    public Text myRankText;
    public Text userName;
    public Text overviewScoreText;
    public Text totalTokenText;
    
    
    
    [Header("DashBoard ")]
    public Text totalWinMatches;
    public Text totalLossMatches;
    public Text bestCareerHigh;
    public Text earnings;
    public Text losses;
    public Text recentRank;
    public Text userNameDashboard;
    public Image customizeSlither;
    public GameObject Losses;
    public GameObject Earnings;
    
    

    [Header("GameOverScreen Text")]
    public Text playerRank;
    public Text tokensEatenGameOverScreen; 
    public Text tokenText;

        [Header("Bools")]
    public bool IsPointerUp;
    
    
    [Header("Enums")]
    public GameMode gameType;


    [Header("Borders")]
    public GameObject leftBorder;
    public GameObject rightBorder;
    public GameObject topBorder;
    public GameObject bottomBorder;
    
    public enum GameMode
    {
      JoyStick,
      Arrow,
      Touch,
      Mouse
        
    }
    
    [Header("GameObjects")]
    [SerializeField] private GameObject leaderBoardParent;
    [SerializeField] private GameObject leaderBoardTextPrefab;
    public GameObject orbParent;
    public GameObject joyStickHandler;
    public GameObject joyStickBackGround;
    public GameObject myScoreParent;


    [Header(("Panels"))] 
    public GameObject addfundPanel;
    public GameObject gameWinPanel;
    public GameObject gameLosePanel;
    public GameObject internnetPopUPpanel;


    [Header("Images")]
    public Image chooseSlitherImage;
    public Image chooseSlitherAIImage;

    
    
    [Header("Camera")]
    public Camera minimapCamera;
    public RectTransform minimap;
    public RawImage rawimage;

    public GameObject orbPrefab;
    [SerializeField]
    private List<GameObject> OrbsList = new List<GameObject>();

    public GameObject bodyPrefab;
    [SerializeField]
    private List<GameObject> bodiesList = new List<GameObject>();
    public IEnumerator PoolOrb()
    {
        for(int i = 0; i < 10000; i++)
        {
            GameObject orb = Instantiate(orbPrefab, orbParent.transform);
            orb.SetActive(false);
            OrbsList.Add(orb);
            if (i % 500 == 0) yield return null;

        }
        
    }
    public GameObject GetOrbs(Vector3 position)
    {
        GameObject orb = null;
        if(OrbsList.Count > 0)
        {
            orb = OrbsList[0];
            orb.transform.position = position;
            orb.transform.rotation = Quaternion.identity;
            orb.SetActive(true);
            OrbsList.RemoveAt(0);
            return orb;
        }

        orb = Instantiate(orbPrefab, position, Quaternion.identity);
        return orb;
    }

    public void DespawnOrb(GameObject orb)
    {
        orb.transform.name = orb.name + "(Deactivated)";
        //orb.transform.parent = null;
        orb.SetActive(false);
        orb.transform.parent = orbParent.transform;
        OrbsList.Add(orb);
    }

    IEnumerator PoolBodyParts()
    {
        for(int i = 0; i< 50000; i++)
        {
            GameObject orb = Instantiate(bodyPrefab,orbParent.transform);
            orb.SetActive(false);
            if (i % 1000 == 0) yield return null;
            bodiesList.Add(orb);
        }
    }
    internal void DespawnBodyParts(GameObject orb)
    {
        orb.transform.name = orb.name + "(Deactivated)";
        //orb.transform.parent = null;
        orb.SetActive(false);
        orb.transform.parent = orbParent.transform;
        bodiesList.Add(orb);
    }

    internal GameObject GetBodyPart(Vector3 position)
    {
        GameObject orb = null;
        if (bodiesList.Count > 0)
        {
            orb = bodiesList[0];
            orb.transform.position = position;
            orb.SetActive(true);
            bodiesList.Remove(orb);
            return orb;
        }

        orb = Instantiate(bodyPrefab, position, Quaternion.identity);
        return orb;
    }
    private void Awake()
    { 
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        
		    #if UNITY_EDITOR
        Debug. unityLogger. logEnabled = true;
    #else
    Debug. unityLogger. logEnabled = false;
    #endif

        // Starts the timer automatically

scores.Clear();
     //  Application.targetFrameRate = 30;

        myScore = 0;
       var a= PlayerPrefs.GetInt("DefaultMode");
       OnClickModes(a);


        StartCoroutine(PoolBodyParts());
        StartCoroutine(PoolOrb());
    }


    public void SetGameData()
    {
        timeRemaining = 600;
        timeAtstart = 600;
        GameManager.instance.gameWinningKills = 3;
        timeSliderValue.maxValue = 600;
    }
    public float timeRemaining = 600;
    public float timeAtstart = 600;
    public bool timerIsRunning = false;


    
    //choose your slither
    public void OnChooseSlitherButtonClicked(int index)
    {
        currentSelectedSlitherIndex+=index;
        if (currentSelectedSlitherIndex>5)
        {
            currentSelectedSlitherIndex = 0;
        }
        else if (currentSelectedSlitherIndex<0)
        {
            currentSelectedSlitherIndex = 5;
        }
        chooseSlitherImage.sprite = chooseSlither[currentSelectedSlitherIndex];
        chooseSlitherAIImage.sprite = chooseSlither[currentSelectedSlitherIndex];

    }

    public void OnClickPlayModes(int index)
    {
        if (index==0)
        {
            GameManager.instance.gameStatus = GameManager.GameStatus.Online;
        }
        else
        { 
            GameManager.instance.gameStatus = GameManager.GameStatus.Offline;
        }
    }
    
    
        public Vector3 mouseInitialPosition;
        public Vector3 mouseCurrentPostion;
        public float distance;
        public bool arrowAssigned;
        public bool arrowDestroy;
        public float deltaTime;
        
        
    void Update()
    {
        
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString ();
         
            if(Input.GetMouseButtonDown(0) && !screens.activeSelf)
			{
				mouseInitialPosition = Input.mousePosition;
                arrowAssigned = false;
                arrowAssigned = true;
            }
			
			if(Input.GetMouseButton(0) && !screens.activeSelf)
			{
                  mouseCurrentPostion = Input.mousePosition;
			      distance = Vector2.Distance(mouseCurrentPostion,mouseInitialPosition);
            }
        
            if (Input.GetMouseButtonUp(0)&& !screens.activeSelf)
            {
                if (gameType==GameMode.Arrow )
                {
                    distance = 0;
                    arrowDestroy = true;
                }
            }
            
            
        if (timerIsRunning)
        {
            if (timeRemaining >1)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                 
//                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
                DisplayTime(timeRemaining);
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);  
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
       timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void OnClickedSprint()
    {
      //  Debug.Log("ispointer pressed");
        IsPointerUp = false;

        
        
    }
    public void OnClickedSprintRelease()
    {
        IsPointerUp = true;
    }

    public float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }
//change text color according to modes
    public void OnClickModes(int modeindex)
    {

        for (int i = 0; i < selectedModeText.Count; i++)
        {
            
       
            selectedModeText[i].color = Color.white;
            selectedModeButtons[i].GetComponent<Image>().color = Color.white;
           
            
        }
        selectedModeText[ modeindex].color=Color.yellow;
        selectedModeButtons[modeindex].GetComponent<Image>().color = Color.yellow;

        currentModeIndex = modeindex;
        MyInfo.defalutMode = modeindex;

        OnClickSaveButton();



    }

    public void OnClickSaveButton()
    {
        if (currentModeIndex==0)
        {
            gameType = GameMode.JoyStick;

        }
        else if (currentModeIndex==1)
        {
            gameType = GameMode.Touch;
            
        }
        else
        {
            gameType = GameMode.Arrow;
            
        }
        
        
//        Debug.Log(currentModeIndex);
        PlayerPrefs.SetInt("DefaultMode", currentModeIndex);
      //  SetPanelActive(8);
        
    }
    
    
    public void OnAddFundPopUpButtonClicked(GameObject panel)
    {
        balanceText.text ="1000000000";
        convertedText.gameObject.SetActive(true);
        convertedAmountText.gameObject.SetActive(false);
        addFundsPopUpButton.interactable = false;
        StartCoroutine(DisablePopUp(2f));

    }

    private IEnumerator DisablePopUp(float time)
    {
        yield return new WaitForSeconds(time);
        addfundPanel.SetActive(false);
    }
    
    
    // addd text in leaderbaord
    public void AddLeaderBoard(String playerName,float myscore)
    {
        
//        Debug.Log("add leaderboard"+ myscore);
        var templeaderboardprefab = Instantiate(leaderBoardTextPrefab, leaderBoardParent.transform);
         
            templeaderboardprefab.GetComponent<Text>().text = myscore.ToString();
            templeaderboardprefab.SetActive(false);
            scores.Add(playerName, myscore);
            scoreTexts.Add(templeaderboardprefab.GetComponent<Text>());
        
           
    }
        
    public void UpdateLeaderBoard(float leaderboardScore, String playerName)
    {
        
     //   Debug.Log("Update leaderboard is called"+ leaderboardScore + "player name "+ playerName);
            foreach (var item in scores)
            {
                if (item.Key==playerName)
                {
                    scores[playerName]  = leaderboardScore/* + item.Value*/;
                   break;
                }
            }

            yourLengthText.text = UI_Manager.instance.myScore.ToString();
            
            var sortedDict = from entry in scores orderby entry.Value descending select entry;
            
              int i = 0;
                foreach (var item in sortedDict)
                {
            //   Debug.Log("score is =" +item.Value.ToString() + "plaeyr name is "+ item.Key + "loop index is= "+i);
                    try
                    {
                scoreTexts[i].text = item.Value.ToString();
                string[] splitArray = item.Key.ToString().Split('_');


                scoreTexts[i].transform.GetChild(0).GetComponent<Text>().text = splitArray[0];//player name

                /*if (!MyInfo.IsOnline())
                {*/
                if (i < 10)
                {
                    scoreTexts[i].gameObject.SetActive(true);


                }
            } catch(Exception e) { }
                    
                    /*}
                    else
                    {
                        scoreTexts[i].gameObject.SetActive(true);

                    }*/
                    
                 //   Debug.Log("item   key"+ item.Key +"local player name"+ MyInfo.localPlayerName);

                  if (item.Key==MyInfo.localPlayerName)
                  {
                      myRank = i+1;
                      
                      yourrankText.text = UI_Manager.instance.myRank.ToString();
                      
                  }
                  
                    i++;
                }
            
            
    }

    
    //delete from leaderboard
    public void OnDeleteFromLeaderBoard(string playerName)
    {

       

        var count = scoreTexts.Count;
        Debug.LogError("destroy  "+ playerName+"snake count " +count);
        
        string[] splitArray = playerName.ToString().Split('_');

        var deleteIndex = 0;
        
        var sortedDict = from entry in scores orderby entry.Value descending select entry;
        
        foreach (var item in sortedDict)
        {
            if (item.Key==playerName)
            {
                Debug.Log("destroy text" + /*scoreTexts[deleteIndex].transform.GetChild(0).GetComponent<Text>().text.ToString()*/ "delete " + deleteIndex);

                scores.Remove(playerName);
                var scoregameobject = scoreTexts[deleteIndex].gameObject;
                scoreTexts.Remove(scoreTexts[deleteIndex]);
                Destroy(scoregameobject);
                break;
            }
            deleteIndex++;
        }
        
      //  for (int i = 0; i < count; i++)
        /*{

          //  if (scores[playerName])
            {
              //  if (scoreTexts[deleteIndex].transform.GetChild(0).GetComponent<Text>().text.ToString() == splitArray[0])
                {
                    Debug.Log("destroy text" + scoreTexts[deleteIndex].transform.GetChild(0).GetComponent<Text>().text.ToString());

                    scores.Remove(playerName);
                    var scoregameobject = scoreTexts[deleteIndex].gameObject;
                    scoreTexts.Remove(scoreTexts[deleteIndex]);
                    Destroy(scoregameobject);
                  
                    return;
                    
                }
            }
        }*/

    }

    

    public void  PlayWithAISetName()//name of player for ai
    {
        if (yourAmountInputFieldAI.text=="" )
        {
            UI_Manager.instance.enterNameErrorAI.text="Please Enter UserName";

            UI_Manager.instance.enterNameErrorAI.gameObject.SetActive(true);
            return;
        }

        
        MyInfo.localPlayerName = yourAmountInputFieldAI.text;
    } 
    public void OnClickAmountToBetButton(Text amountToBet)
    { 
        yourAmountInputField.text = amountToBet.text.ToString();
        
    } 
    public void OnClickAddFundButton(Text amountToBet)//for add fund pop up
    { 
        addFundPopUpInputField.text = amountToBet.text.ToString();
    }
    
    public IEnumerator WaitForGivenTime(float time)
    {
        yield return new WaitForSeconds(time);



        if (leaderBoardParent.transform.childCount>0)
        {
            foreach (Transform child in leaderBoardParent.transform) {
                GameObject.Destroy(child.gameObject);
            }
        }
      
        
         

         

            timerIsRunning = false;
            //survivalTimeText.text = (timeAtstart-timeRemaining).ToString()+"s";
            playerKilledText.text = GameManager.instance.localPlayerKills.ToString();

            var timeToDisplay = timeAtstart - timeRemaining;
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            GameManager.instance.gameOver = true;
            survivalTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            tokenEaten.text =  myScore.ToString();;
            tokensEatenGameOverScreen.text =  myScore.ToString();;
            myRankText.text = myRank.ToString();

            playerRank.text = myRank.ToString();

            timeSliderValue.value = timeToDisplay;
          //  Debug.LogError("local plaeyr name+" + MyInfo.localPlayerName);

             
                string[] splitArray = MyInfo.localPlayerName.ToString().Split('_');
                userName.text = splitArray[0];

            

            overviewScoreText.text = myScore.ToString();
            SetPanelActive(2);
            screens.SetActive(true);

 

            yield return new WaitForSeconds(1f);
             
       
    }


    public IEnumerator StopLoading()
    {
        yield return new WaitForSeconds(3f);
        if (GameManager.instance.gameOver == false)
        {
            Debug.LogError("Loadung pel deactivated");
            UI_Manager.instance.screens.SetActive(false);
        }



    }
    
    public void SetPanelActive(int panelId)
    {
        if (panelId==1 || panelId==9)
        {
            backButton.gameObject.SetActive(true);
            settingButton.gameObject.SetActive(true);
        }
        else
        {
            backButton.gameObject.SetActive(false);
            settingButton.gameObject.SetActive(false);
        }
        for (int i = 0; i < panelInfo.Count; i++)
        {
            panelInfo[i].panel.SetActive(false);
        }
        Debug.LogError(panelInfo[panelId].panelName + " activating");
        panelInfo[panelId].panel.SetActive(true);

    }
    
    
    

    //add funds popup
    public void OnValueChangeAddFunds(InputField inputvalues)
    {
        
         
        if (float.Parse(inputvalues.text)>0.0f)
        {
            addFundsPopUpButton.interactable = true;
            convertedAmountText.gameObject.SetActive(true);
            convertedText.gameObject.SetActive(false);
        }
        else
        {
            addFundsPopUpButton.interactable = false;
            convertedAmountText.gameObject.SetActive(false);
            convertedText.gameObject.SetActive(true);
        }
        
    }
    public void OnClickPlayAgain()
    {
        SceneManager.LoadScene("TopDodge");
    }

    
}

[System.Serializable]
public class UIPanels
{
    public  GameObject panel;
    public string panelName;
}
