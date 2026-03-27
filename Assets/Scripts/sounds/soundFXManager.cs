using UnityEngine;

public class soundFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource soundObject;
    public static soundFXManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void playSoundClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
