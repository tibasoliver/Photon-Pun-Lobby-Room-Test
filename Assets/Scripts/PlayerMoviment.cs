using Photon.Pun;
using UnityEngine;

public class PlayerMoviment : MonoBehaviour
{
    public float velocity = 1.5f;
    public float rotation = 90.0f;
    PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GeneralManager.TimerActive && photonView.IsMine && photonView.ControllerActorNr == photonView.CreatorActorNr)
        {
            float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Vertical");

            if(x == 0f)
            {
                if (Input.GetAxis("Horizontal2") > 0f)
                {
                    x += 1f;
                }
                else if(Input.GetAxis("Horizontal2") < 0f)
                {
                    x -= 1f;
                }
            }

            if (y == 0f)
            {
                if (Input.GetAxis("Vertical2") > 0f)
                {
                    y += 1f;
                }
                else if (Input.GetAxis("Vertical2") < 0f)
                {
                    y -= 1f;
                }
            }

            //Vector3 dir = new Vector3(-y, 0, x)*velocity;
            Vector3 dir = new Vector3(-y, 0, x).normalized * velocity;

            transform.Translate(dir * Time.deltaTime);
        }
    }
}
