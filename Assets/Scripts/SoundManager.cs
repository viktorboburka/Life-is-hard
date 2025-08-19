using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;
    [SerializeField] public AudioSource musicSource;
    [SerializeField] AudioSource click;
    [SerializeField] AudioSource cameraSound;
    float lastClickPlayedAt = 0f;

    internal void PlayCameraSound()
    {
        cameraSound.Play();
    }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {

    }

    public void PlayClickSound() {
        if (Time.timeSinceLevelLoad - lastClickPlayedAt < 0.25f) return;
        click.Play();
        lastClickPlayedAt = Time.timeSinceLevelLoad;
    }

    public void PlayLetterClickSound() {
        click.Play();
    }
}
