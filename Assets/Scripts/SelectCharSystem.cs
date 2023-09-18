using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class SelectCharSystem : MonoBehaviourPunCallbacks
{
    public GameObject spawner;//your position scene
    public GameObject rival;//rival position scene

    public GameObject[] listChar = new GameObject[4];
    public GameObject CharActive;
    public GameObject RivalCharActive;
    public int idRival = 0;
    public int idChar = 0;

    public TextMeshProUGUI txt_MeshCharNameSelected;
    private TMP_Text txt_CharNameSelected;

    public GameData gameData;

    public Player rivalPlayer;


    // Update is called once per frame
    void Update()
    {
        if (GeneralManager.IsCharselectSceneActive && GeneralManager.CharSelectionShowedOnce==false)
        {
            InstantiateChar();//happens once
            GeneralManager.IsCharselectSceneActive = false;
            GeneralManager.CharSelectionShowedOnce = true;
        }
    }

    public void InstantiateChar()
    {
        CharActive = Instantiate(listChar[idChar], spawner.transform.position, spawner.transform.rotation);//instantiates own char

        //updates info char network
        Hashtable props = new Hashtable
        {
            { "CHARACTER_ID", 0 }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        updateCharId(idChar);//updates info own char

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && RivalCharActive == null)// rival wasn't instantiated
        {
            int? rivalId = getRivalId();
            
            if (rivalId != null)
            {
                RivalCharActive = Instantiate(listChar[(int)rivalId], rival.transform.position, rival.transform.rotation);
            }
            else
            {
                RivalCharActive = Instantiate(listChar[0], rival.transform.position, rival.transform.rotation);
            }
        }

        txt_CharNameSelected = txt_MeshCharNameSelected.GetComponent<TMP_Text>();
        
        //shows name selected char
        txt_CharNameSelected.text = listChar[idChar].GetComponent<PlayerConfig>().playerData.nameChar;
    }

    
    private int? getRivalId()
    {

        Dictionary<int, Player> players = PhotonNetwork.CurrentRoom.Players;

        //Identifies other player
        Player otherPlayer = null;

        foreach (KeyValuePair<int, Player> playerInfo in players)
        {
            if (playerInfo.Value != PhotonNetwork.LocalPlayer)
            {
                otherPlayer = playerInfo.Value;
                break;
            }
        }

        if (otherPlayer != null)
        {
            // Tries to obtain custom attribute "CHARACTER_ID"
            object rivalAttribute;
            if (otherPlayer.CustomProperties.TryGetValue("CHARACTER_ID", out rivalAttribute))
            {
                if (rivalAttribute != null)
                    invUpdateCharId((int)rivalAttribute);

                return (int)rivalAttribute;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    
    public void ChangeToNextCharacter()
    {
        GameObject charTemp = CharActive;
        Destroy(charTemp);

        CharActive = null;


        if (idChar < listChar.Length -1)
            ++idChar;
        else
            idChar = 0;

        Hashtable props = new Hashtable
        {
            { "CHARACTER_ID", idChar }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);//updates on network

        updateCharId(idChar);//updates locally

        CharActive = Instantiate(listChar[idChar], spawner.transform.position, spawner.transform.rotation);//instantiates char model

        txt_CharNameSelected.text = listChar[idChar].GetComponent<PlayerConfig>().playerData.nameChar;//change text of selected char

    }

    public void ChangeToPreviousCharacter()
    {
        GameObject charTemp = CharActive;
        Destroy(charTemp);

        CharActive = null;


        if (idChar > 0)
            --idChar;
        else
            idChar = listChar.Length-1;

        Hashtable props = new Hashtable
        {
            { "CHARACTER_ID", idChar }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        updateCharId(idChar);

        CharActive = Instantiate(listChar[idChar], spawner.transform.position, spawner.transform.rotation);

        txt_CharNameSelected.text = listChar[idChar].GetComponent<PlayerConfig>().playerData.nameChar;
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player " + otherPlayer.NickName + " left room.");
        Debug.Log("Player " + otherPlayer.ActorNumber + " left room.");
        
        if(otherPlayer.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            GameObject charTemp = RivalCharActive;
            Destroy(charTemp);
            RivalCharActive = null;

            //behavior is inverted because it is called
            //after recalculating roles
            if (PhotonNetwork.IsMasterClient)
            {
                gameData.charIdMasterCLient = gameData.charIdClient;
                gameData.charIdClient = 0;
                idRival = 0; 
            }
            else
            {
                gameData.charIdClient = 0;
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && RivalCharActive == null)
        {       
            //Just first time, and after that will get
            //from update
            idRival = 0;

            if (PhotonNetwork.IsMasterClient)
            {
                idRival = gameData.charIdClient;
            }
            else
            {
                //aqui a sala precisva ter alguma informaçao?
                idRival = gameData.charIdMasterCLient;
            }
            RivalCharActive = Instantiate(listChar[idRival], rival.transform.position, rival.transform.rotation);
        }
       
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {

        if (targetPlayer.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            object rivalCharacterId;
            if(changedProps.TryGetValue("CHARACTER_ID",out rivalCharacterId))
            {
                if(idRival != (int)rivalCharacterId)
                {
                    idRival = (int)rivalCharacterId;

                    invUpdateCharId(idRival);

                    GameObject charTemp = RivalCharActive;
                    Destroy(charTemp);
                    RivalCharActive = null;
                    RivalCharActive = Instantiate(listChar[idRival], rival.transform.position, rival.transform.rotation);
                }
            }
            
        }
    }

    public void updateCharId(int id)
    {
        //sets own info
        if (PhotonNetwork.IsMasterClient)
        {
            gameData.charIdMasterCLient = id;
        }
        else
        {
            gameData.charIdClient = id;
        }
    }

    public void invUpdateCharId(int id)
    {
        //sets rival info
        if (PhotonNetwork.IsMasterClient)
        {
            gameData.charIdClient = id;
        }
        else
        {
            gameData.charIdMasterCLient = id;
        }
    }
}


