using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameData : MonoBehaviour, IPunObservable
{
    public int charIdMasterCLient;
    public int charIdClient;
    public bool sorted;
    public int positionNumber;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(charIdMasterCLient);
            stream.SendNext(charIdClient);
            stream.SendNext(sorted);
            stream.SendNext(positionNumber);
        }
        else
        {
            charIdMasterCLient = (int)stream.ReceiveNext();
            charIdClient = (int)stream.ReceiveNext();
            sorted = (bool)stream.ReceiveNext();
            positionNumber = (int)stream.ReceiveNext();
        }
    }
}
