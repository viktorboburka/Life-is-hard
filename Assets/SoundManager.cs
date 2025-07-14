using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;
    [SerializeField] public AudioSource musicSource;

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
}
