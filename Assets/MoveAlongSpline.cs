using UnityEngine;
using UnityEngine.Splines;

public class MoveAlongSpline : MonoBehaviour
{

    [SerializeField] KeyCode key;
    bool lastKeyPressed;
    float progress = 0f;


    float maxSpeed = 0.02f;
    float minSpeed = 0.01f;
    float speed = 0f;
    float acceleration = 0.01f;
    SplineAnimate splineAnimate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineAnimate = GetComponent<SplineAnimate>();
    }

    // Update is called once per frame
    void Update()
    {
        bool currentKeyPress = Input.GetKey(key);
        if (currentKeyPress) {
            if (!lastKeyPressed) {
                speed = 0f;
            }
            speed += acceleration * Time.deltaTime;
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

            progress += speed;
        }
        else {
            if (lastKeyPressed) {
                speed = 0f;
            }
            speed += acceleration * Time.deltaTime;
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

            progress -= speed;
        }
        progress = Mathf.Clamp(progress, 0f, 0.999f);

        splineAnimate.ElapsedTime = progress;

        lastKeyPressed = Input.GetKey(key);
    }
}
