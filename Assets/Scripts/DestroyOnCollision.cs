using UnityEngine;
using Photon.Pun;

public class DestroyOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
        {
            PhotonView photoView = collision.gameObject.GetComponent<PhotonView>();
            if (photoView.IsMine)
            {
                GeneralManager.pointsP1 += 10;
            }
            else
            {
                GeneralManager.pointsP2 += 10;
            }
            Destroy(this.gameObject);
        }
    }
}
