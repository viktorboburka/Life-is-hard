using System;
using UnityEngine;
using UnityEngine.Splines;

public class MoveAlongSpline : MonoBehaviour
{

    [SerializeField] KeyCode key;
    bool lastKeyPressed;
    public float progress = 0f;


    float maxSpeed = 0.02f;
    float minSpeed = 0.01f;
    float speed = 0f;
    float acceleration = 0.01f;
    SplineAnimate splineAnimate;

    float tapMaxSpeed = 0.15f;
    float tapMinSpeed = -0.15f;
    float tapSpeed = 0f;
    float tapAcceleration = 0.003f;
    float tapDecceleration = 0.005f;

    public bool doneMoving = false;

    void Start()
    {
        splineAnimate = GetComponent<SplineAnimate>();
    }

    void Update()
    {
        if (doneMoving) return;
        //HoldControlsUpdate();
        TapControlsUpdate();
    }
    
    void TapControlsUpdate() {
        bool currentKeyPress = Input.GetKeyDown(key);
        if (currentKeyPress) {
            tapSpeed += tapAcceleration;
            tapSpeed = Mathf.Clamp(tapSpeed, tapMinSpeed, tapMaxSpeed);

            progress += tapSpeed;
        }
        else {
            tapSpeed -= tapDecceleration * Time.deltaTime;
            //decceleration is 2x faster if the lip is moving up
            if (tapSpeed > 0f) tapSpeed -= tapDecceleration * Time.deltaTime;
            tapSpeed = Mathf.Clamp(tapSpeed, tapMinSpeed, tapMaxSpeed);

            progress += tapSpeed;
        }
        progress = Mathf.Clamp(progress, 0f, 0.999f);
        if (progress >= 0.998f) tapSpeed /= 2;
        if (progress < 0.001f) tapSpeed = 0;

        splineAnimate.ElapsedTime = progress;

        lastKeyPressed = Input.GetKey(key);
    }

    void HoldControlsUpdate()
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
