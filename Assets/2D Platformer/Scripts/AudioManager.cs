using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip BackgroundSound;
    private void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] AudioSource SFXobj;
    public void PlayAudio(AudioClip audioClip, Vector3 pos, float volume = 1.0f)
    {
        AudioSource audioSource = Instantiate(SFXobj, pos, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    private void Start()
    {
        StartCoroutine(PlayBGAudio());
    }

    IEnumerator PlayBGAudio()
    {
        while (true)
        {
            PlayAudio(BackgroundSound, transform.position, .2f);
            yield return new WaitForSeconds(BackgroundSound.length);
        }
    }
}
