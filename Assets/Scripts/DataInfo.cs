using UnityEngine;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class DataInfo : MonoBehaviour
{
    public Player[] players = new Player[2];
    public Hashtable[] customProperties = new Hashtable[2];
    public bool Updated = false;

    public int charId;
    public int rivalCharId;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void updatePropertiesLocalPalyerNetwork()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetCustomProperties(customProperties[i]);
            Updated = true;
        }
    }
}
