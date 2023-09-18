using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ShowPing : MonoBehaviour
{
    public TextMeshProUGUI MeshPing;
    private TMP_Text txt_ping;

    // Start is called before the first frame update
    void Start()
    {
        txt_ping = MeshPing.GetComponent<TMP_Text>();
        StartCoroutine(RunUpdatePing());
    }

    IEnumerator RunUpdatePing()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            UpdatePing();
        }
    }

    void UpdatePing()
    {
        if (PhotonNetwork.IsConnected)
        {
            txt_ping.text = "Ping: " + PhotonNetwork.GetPing().ToString() + " ms";
        }
        else
        {
            txt_ping.text = "Ping: --- ms";
        }
    }
}
