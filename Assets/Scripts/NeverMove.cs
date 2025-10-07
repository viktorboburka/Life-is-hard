using UnityEngine;

public class NeverMove : MonoBehaviour
{

    Vector3 originalPos;
    Quaternion originalRot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = originalPos;
        transform.rotation = originalRot;
    }
}
