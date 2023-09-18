using Photon.Pun;
using UnityEngine;

public class MyPlayer : MonoBehaviour,IPunObservable
{
    public Transform transformOfPlayer;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        transformOfPlayer = gameObject.transform;
    }
}
