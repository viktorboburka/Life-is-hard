using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;
    [SerializeField] public AudioSource musicSource;
    [SerializeField] AudioSource click;
    [SerializeField] AudioSource cameraSound;

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
        click.Play();
    }

    public void PlayLetterClickSound() {
        click.Play();
    }
}
