using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkSystem : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public TMP_InputField playerNameInput;
    public TextMeshProUGUI meshStatusDescription;
    public TMP_Text statusDescription;
    public Button connectToServer;
    string playerNameTemp;

    string roomName;

    public Canvas canvasCreateEnterRoom;
    public Canvas canvasSelectChar;
    public Canvas canvasConnectionLostAtRoom;
    public GameObject plane;

    public Button buttonAmIReady;
    public Button buttonNextChar;
    public Button buttonPrevious;
    bool iAmReady = false;

    bool isLobbyOrRoomScene = true;

    int sortPosition = -1;

    public GameData gameData;

    public const string STATUS_ROOM = "isopen";


    enum StatusConnection
    {
        ConnectingToNameServer = 0,
        ConnectingToMasterServer = 1,
        EnteringLobby = 2,
        JoiningRoom = 3,
        FailedToConnect = 4,
        ExitLobby = 5,
        CreatingRoom = 6,
        JoinedRoom = 7,
        EnteredGameScene = 8,
        EndGame = 9
    }

    StatusConnection status;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        SceneManager.sceneLoaded += OnSceneLoaded;
        gameData = GameObject.Find("InfoTransferNextScene").GetComponent<GameData>();
    }

    // Start is called before the first frame update
    void Start()
    {
        statusDescription = meshStatusDescription.GetComponent<TMP_Text>();
        playerNameTemp = "Player" + UnityEngine.Random.Range(1000, 10000);

        //init with yellow color
        Image imageButton = buttonAmIReady.GetComponent<Image>();
        Color yellow = new Color(245, 243, 91, 255);
        yellow /= 255.0f;
        imageButton.color = yellow;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnConnected()
    {
        Debug.Log("OnConnected");
        status = StatusConnection.ConnectingToMasterServer;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");

        status = StatusConnection.EnteringLobby;
        Debug.Log("Server: " + PhotonNetwork.CloudRegion + " Ping: " + PhotonNetwork.GetPing());
        PhotonNetwork.JoinLobby();

        if (isLobbyOrRoomScene)
            statusDescription.text = "Status: Conectado ao servidor";

        status = StatusConnection.EnteringLobby;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");

        if (isLobbyOrRoomScene)
            statusDescription.text = "Status: Entrou no Lobby";

        Hashtable expectedCustomRoomProperties = new Hashtable { { STATUS_ROOM, 1 } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 2);
        status = StatusConnection.JoiningRoom;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CleanupCacheOnLeave = false;
        roomOptions.MaxPlayers = 2;
        roomOptions.CustomRoomPropertiesForLobby = new string[]{ STATUS_ROOM };
        roomOptions.CustomRoomProperties = new Hashtable { {STATUS_ROOM, 1} };

        PhotonNetwork.CreateRoom(null, roomOptions, null);
        
        status = StatusConnection.CreatingRoom;
    }

    public override void OnLeftLobby()
    {

    }

    public override void OnJoinedRoom()
    {

        Hashtable props = new Hashtable
        {
            { "CHARACTER_ID", 0},
            { "AMIREADY_ID", false }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);//updates on network

        if (PhotonNetwork.InRoom)
        {
            string roomName = PhotonNetwork.CurrentRoom.Name;
            if (status == StatusConnection.JoiningRoom)
            {
                Debug.Log("Entrou na room: " + roomName);
                Debug.Log(PhotonNetwork.NickName);
            }
            else
            {
                Debug.Log("Criou room: " + roomName);
                Debug.Log(PhotonNetwork.NickName);
            }

            status = StatusConnection.JoinedRoom;

            if (isLobbyOrRoomScene)
            {
                statusDescription.fontSize = 28;//before 50
                statusDescription.text = "Status: Conectado ao Room - " + roomName;
            }

            //show properties from current room
            Debug.Log("Room Data: \n"+PhotonNetwork.CurrentRoom.CustomProperties.ToString());

        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && GeneralManager.CharSelectionShowedOnce == false)
        {
            ShowSelectArea();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (cause.ToString() == "DnsExceptionOnConnect")
        {
            if (isLobbyOrRoomScene)
                statusDescription.text = "Status: Falha ao conectar ao servidor";

            status = StatusConnection.FailedToConnect;

            if (isLobbyOrRoomScene)
                connectToServer.enabled = true;
        }
        else if (status == StatusConnection.JoinedRoom && canvasSelectChar.enabled == true)
        {
            if (isLobbyOrRoomScene)
            {
                canvasCreateEnterRoom.gameObject.SetActive(false);
                canvasSelectChar.gameObject.SetActive(false);
            }

            canvasConnectionLostAtRoom.gameObject.SetActive(true);
        }
        else if (cause.ToString() == "ClientTimeout")
        {
            if (isLobbyOrRoomScene)
                statusDescription.text = "Status: Conexão Perdida - Timeout";

            status = StatusConnection.FailedToConnect;

            if (isLobbyOrRoomScene)
                connectToServer.enabled = true;
        }
        Debug.Log("OnDisconneced: " + cause);
    }

    public void Login()
    {
        if (isLobbyOrRoomScene)
            connectToServer.enabled = false;

        PhotonNetwork.ConnectUsingSettings();
        getNickName();
        status = StatusConnection.ConnectingToNameServer;

        if (isLobbyOrRoomScene)
            statusDescription.text = "Status: Conectando ao servidor";
    }

    public void getNickName()
    {
        if (playerNameInput.text != "")
        {
            PhotonNetwork.NickName = playerNameInput.text;
        }
        else
        {
            PhotonNetwork.NickName = playerNameTemp;

            if (isLobbyOrRoomScene)
                playerNameInput.text = playerNameTemp;
        }
    }

    public void EntrarCriarSala()
    {
        string roomNameTemp = "Room" + UnityEngine.Random.Range(1000, 10000);
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom(roomNameTemp, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("O jogador " + newPlayer.NickName + " entrou na sala");

        ShowSelectArea();
    }

    public void ShowSelectArea()
    {
        if (isLobbyOrRoomScene)
        {
            plane.SetActive(true);
            canvasCreateEnterRoom.gameObject.SetActive(false);
            canvasSelectChar.gameObject.SetActive(true);
        }

        GeneralManager.IsCharselectSceneActive = true;
    }

    public void ReturnToLoginArea()
    {
        GeneralManager.Reset();
        Destroy(gameData.gameObject);
        SceneManager.LoadScene("OnlineGame");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left");

        if (status == StatusConnection.JoinedRoom && canvasSelectChar.enabled == true && isLobbyOrRoomScene)
        {
            canvasCreateEnterRoom.gameObject.SetActive(false);
            canvasSelectChar.gameObject.SetActive(false);
            canvasConnectionLostAtRoom.gameObject.SetActive(true);
        }
        else if (status == StatusConnection.EnteredGameScene && !isLobbyOrRoomScene)
        {
            canvasConnectionLostAtRoom.gameObject.SetActive(true);
        }
    }

    public void ChangeStatusReady()
    {
        Color yellow = new Color(245, 243, 91, 255);
        Color green = new Color(109, 255, 85, 255);

        yellow /= 255.0f;
        green /= 255.0f;

        Image imageButton = buttonAmIReady.GetComponent<Image>();

        if (imageButton.color.Compare(yellow))
        {
            imageButton.color = green;
            iAmReady = true;
            informStatus(true);
        }
        else
        {
            imageButton.color = yellow;
            iAmReady = false;
            informStatus(false);
        }
    }

    public void informStatus(bool status)
    {
        setStatusReady(status);
        buttonNextChar.interactable = !status;
        buttonPrevious.interactable = !status;
    }

    public void setStatusReady(bool isReady)
    {
        Hashtable props = new Hashtable
        {
            { "AMIREADY_ID", isReady }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (iAmReady)
        {
            object statusTargetPlayer;
            changedProps.TryGetValue("AMIREADY_ID", out statusTargetPlayer);

            if (changedProps.ContainsKey("AMIREADY_ID") && PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                CheckAllPlayersReady();
            }
        }
        else
        {
            Debug.Log("Player não sinalizou estar pronto");
        }
    }

    private void CheckAllPlayersReady()
    {
        Player[] players = PhotonNetwork.PlayerList;

        bool allPlayersReady = true;

        foreach (Player player in players)
        {
            object isReady;
            if (player.CustomProperties.TryGetValue("AMIREADY_ID", out isReady))
            {

                if ((bool)isReady == false)
                {
                    allPlayersReady = false;
                    break;
                }
            }
            else
            {
                allPlayersReady = false;
                break;
            }
        }

        if (allPlayersReady)
        {
            Debug.Log("Todos os jogadores estão prontos!");
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(canvasConnectionLostAtRoom);

            Hashtable customRoomProperties = new Hashtable
            {
                { STATUS_ROOM, 0 }
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);

            LoadSceneByMasterClient();
        }
        else
        {
            Debug.Log("Ainda não todos os jogadores estão prontos.");
        }
    }

    public void LoadSceneByMasterClient()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isLobbyOrRoomScene = false;
            status = StatusConnection.EnteredGameScene;

            //Sort Master Client Position e set information his position on MasterClient
            sortPosition =  UnityEngine.Random.Range(0, 2);

            Hashtable props = new Hashtable
            {
                { "POSITION_ID", sortPosition },
                { "SORTED", true } //even change master client we have info who sorted
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            gameData.sorted = true;
            gameData.positionNumber = sortPosition;

            PhotonNetwork.LoadLevel("CatchCoins");
        }
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (scene.name == "OnlineGame" && (status == StatusConnection.EnteredGameScene || status == StatusConnection.FailedToConnect))
        {
            Destroy(canvasConnectionLostAtRoom.gameObject);
            Destroy(this.gameObject);
        }

        if(scene.name == "CatchCoins")
        {
            status = StatusConnection.EnteredGameScene;
            isLobbyOrRoomScene = false;

            Hashtable props = new Hashtable
            {
                { "CHARACTER_ID", 0},
                { "AMIREADY_ID", false }
            };

            //updates on network when enters game, clean variables from player
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
    }
}
