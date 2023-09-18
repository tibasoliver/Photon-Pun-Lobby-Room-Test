using Photon.Pun;
using UnityEngine;

public class InstantiationSystem : MonoBehaviour, IPunObservable
{
    public Transform top_left;
    public Transform top_right;
    public Transform bottom_left;
    public Transform bottom_right;

    public GameObject gameobject;
    public GameObject parent;
    public int timeGameObjectInstantiated = 10;

    public float deltaBetweenInstantiation = 5.0f;
    public float nextInstantiation;

    public PhotonView photonView;

    void Start()
    {
        nextInstantiation = GeneralManager.countdownInitial;
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if(nextInstantiation > GeneralManager.countdown && PhotonNetwork.IsMasterClient)
        {
            //int h = 24;
            //int v = 11;
            Vector3 horizontal_diff = new Vector3(0,0, top_right.position.z - top_left.position.z);
            Vector3 vertical_diff = new Vector3(bottom_left.position.x - top_left.position.x, 0, 0);

            for (int i = timeGameObjectInstantiated; i > 0; i--)
            {
                int randomH = Random.Range(0, 24);
                int randomV = Random.Range(0, 11);

                Vector3 position_a = top_left.position + horizontal_diff / (24 * 2) + 
                    (horizontal_diff * randomH) / 24 + vertical_diff / (11 * 2) + (vertical_diff * randomV) / 11;
                Quaternion rotation = Quaternion.identity;
                GameObject instance = PhotonNetwork.InstantiateRoomObject("Apple_NFT", position_a, rotation);
            }

            nextInstantiation -= deltaBetweenInstantiation;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(nextInstantiation);
        }
        else
        {
            nextInstantiation = (float)stream.ReceiveNext();
        }
    }
}
