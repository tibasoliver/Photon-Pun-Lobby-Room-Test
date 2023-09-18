using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private Vector3 rotationSpeed = new Vector3(0, 100, 0);  

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
