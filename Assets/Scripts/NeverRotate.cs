using UnityEngine;

public class NeverRotate : MonoBehaviour
{

    Vector3 originalLocalPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalLocalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        transform.localPosition = originalLocalPos;
    }
}
