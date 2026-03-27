using UnityEngine;

public class musicManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicObject;
    private AudioSource currentMusic;
    public static musicManager instance;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    // TODO: Change only audio clip instead of replacing/deleting music object
    public void playMusicBG(AudioClip audioClip, Transform spawnTransform, float volume)
    {

        if (currentMusic != null)
        {
            if (audioClip == currentMusic.clip) // if music plays same audio name, skip execution
            {
                return;
            }
            Destroy(currentMusic.gameObject);
        }

        AudioSource audioSource = Instantiate(musicObject, spawnTransform.position, Quaternion.identity);
        currentMusic = audioSource; // save current audio object

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        // float clipLength = audioSource.clip.length;
    }
}
