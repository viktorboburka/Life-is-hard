using UnityEngine;

public class NeverMove : MonoBehaviour
{

    Vector3 originalPos;
    Quaternion originalRot;

    Vector3 parentLastPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
        parentLastPos = transform.parent.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.position -= transform.parent.position - parentLastPos;
        transform.position = originalPos;
        transform.rotation = originalRot;
        parentLastPos = transform.parent.position;
    }

    void Update() {
        LateUpdate();
    }
}
