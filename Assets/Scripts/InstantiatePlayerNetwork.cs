using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InstantiatePlayerNetwork : MonoBehaviour
{
    public int sortedPositionMasterClient;
    public int charId;

    public Transform p1;
    public Transform p2;

    public GameObject[] listChar = new GameObject[4];

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            Player sorter = discoverPlayerSorted();

            object sortedNumber;
            sorter.CustomProperties.TryGetValue("POSITION_ID", out sortedNumber);
            sortedPositionMasterClient = (int)sortedNumber;

            object pId;
            PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("CHARACTER_ID", out pId);
            charId = (int)pId;

            if (sorter == PhotonNetwork.LocalPlayer)//own position
            {
                if (sortedPositionMasterClient == 0)
                {
                    GameObject p = listChar[charId];
                    PhotonNetwork.Instantiate(listChar[charId].name, p1.position, p1.rotation);
                }
                else
                {
                    GameObject p = listChar[charId];
                    PhotonNetwork.Instantiate(listChar[charId].name, p2.position, p2.rotation);
                }
            }
            else
            {
                if (sortedPositionMasterClient == 0)
                {
                    GameObject p = listChar[charId];
                    PhotonNetwork.Instantiate(listChar[charId].name, p2.position, p2.rotation);
                }
                else
                {
                    GameObject p = listChar[charId];
                    PhotonNetwork.Instantiate(listChar[charId].name, p1.position, p1.rotation);
                }
            }
        }
    }

    public Player discoverPlayerSorted()
    {
        Player[] players = PhotonNetwork.PlayerList;
        Player sorter = null;

        foreach (Player player in players)
        {
            object isReady;
            if (player.CustomProperties.TryGetValue("SORTED", out isReady))
            {

                if ((bool)isReady == true)
                {
                    sorter = player;
                    break;
                }
                else
                {
                    continue;
                }
            }
            else
            {
                continue;
            }
        }

        return sorter;
    }
}
