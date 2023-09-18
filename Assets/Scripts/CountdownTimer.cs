using System.Collections;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviourPunCallbacks, IPunObservable
{
    public float timeRemaining = 20.0f;
    public bool timerIsRunning = false;
    public TextMeshProUGUI textMesh;
    private TMP_Text txt;
    public TextMeshProUGUI txt_MeshPoints;
    private TMP_Text txt_points;
    public bool playerInstantiated = false;

    public GameData gameData;
    public GameObject[] listChar = new GameObject[4];

    public Transform p1;
    public Transform p2;

    public bool endGame = false;
    public GameObject canvasEndGame;
    public TextMeshProUGUI gameResult;
    private TMP_Text gameResultText;

    public GameObject canvasLostConnection;

    bool returnToMenu = false;

    public TextMeshProUGUI showScore;
    private TMP_Text showScoreText;

    // Start is called before the first frame update
    void Awake()
    {
        txt = textMesh.GetComponent<TMP_Text>();
        txt_points = txt_MeshPoints.GetComponent<TMP_Text>();
        UpdateDisplay();


        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(waitForSeconds(2f));
            timerIsRunning = true;
        }//considered that all players are ready
        //the right would be syncronize all player e each one says 'I am ready for a short period

        GeneralManager.TimerActive = true;
        GeneralManager.countdownInitial = timeRemaining;
    }

    void Start()
    {
        GameObject data = GameObject.Find("InfoTransferNextScene");
        gameData = data.GetComponent<GameData>();

        gameResultText = gameResult.GetComponent<TMP_Text>();
        showScoreText = showScore.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (PhotonNetwork.IsMasterClient)
                timeRemaining -= Time.deltaTime;

            if (timeRemaining >= 0)
            {
                UpdateDisplay();
            }
            else
            {
                timerIsRunning = false;
                GeneralManager.TimerActive = false;
            }
        }

        if (timeRemaining != 120f && playerInstantiated == false && gameData.sorted==true)//ask if updated sorted info from Master
        {
            if (PhotonNetwork.IsMasterClient)
            {

                PhotonNetwork.Instantiate(listChar[gameData.charIdMasterCLient].name,
                    gameData.positionNumber == 0 ? p1.transform.position : p2.transform.position,
                    gameData.positionNumber == 0 ? p1.transform.rotation : p2.transform.rotation);

            }
            else
            {
                PhotonNetwork.Instantiate(listChar[gameData.charIdClient].name,
                    gameData.positionNumber == 0 ? p2.transform.position : p1.transform.position,
                    gameData.positionNumber == 0 ? p2.transform.rotation : p1.transform.rotation);
            }
            playerInstantiated = true;
        }

        if (!endGame)
        {
            if (timeRemaining <= 0f)
            {
                GeneralManager.TimerActive = false;

                canvasEndGame.SetActive(true);
                if (GeneralManager.pointsP1 > GeneralManager.pointsP2)
                    gameResultText.text = "YOU WIN!";
                else if (GeneralManager.pointsP1 < GeneralManager.pointsP2)
                    gameResultText.text = "YOU LOSE!";
                else
                    gameResultText.text = "DRAWN GAME!";

                int scoreStored = PlayerPrefs.GetInt("ScorePoints", 0);
                if (GeneralManager.pointsP1 <= scoreStored)
                {
                    showScoreText.text = "HighScore : " + string.Format("{0:000}", scoreStored);
                }
                else
                {
                    showScoreText.text = "New HighScore : " + string.Format("{0:000}", GeneralManager.pointsP1);
                    PlayerPrefs.SetInt("ScorePoints", GeneralManager.pointsP1);
                    PlayerPrefs.Save();
                }

                GeneralManager.pointsP1 = 0;
                GeneralManager.pointsP2 = 0;

                PhotonNetwork.AutomaticallySyncScene = false;

                endGame = true;
            }
        }

    }

    private void UpdateDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        txt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        GeneralManager.countdown = timeRemaining;

        txt_points.text = string.Format("YOU: {0:000}   COM: {1:000}", GeneralManager.pointsP1, GeneralManager.pointsP2);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(timeRemaining);
            stream.SendNext(timerIsRunning);
        }
        else
        {
            timeRemaining = (float)stream.ReceiveNext();
            timerIsRunning = (bool)stream.ReceiveNext();
        }
    }

    private IEnumerator waitForSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("2 seconds gone!");
    }

    public void callReturnAreaFunction()
    {
        returnToMenu = true;
        PhotonNetwork.Disconnect();
    }

    public override void OnJoinedRoom()
    {

    }

    public override void OnLeftRoom()
    {
        if (returnToMenu)
        {
            GameObject network = GameObject.Find("NetworkSystem");
            Destroy(network.gameObject.GetComponent<NetworkSystem>().canvasConnectionLostAtRoom.gameObject);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        GeneralManager.Reset();
        if (returnToMenu)
        {
            GameObject info = GameObject.Find("InfoTransferNextScene");
            Destroy(info.gameObject);

            GameObject network = GameObject.Find("NetworkSystem");
            Destroy(network.gameObject);

            SceneManager.LoadScene(0);
        }
    }


}
