using UnityEngine;

public class AmbianceSounds : MonoBehaviour
{
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip[] audioClips;

    [SerializeField] private float minTimeBetweenSounds;
    [SerializeField] private float maxTimeBetweenSounds;

    private float timer;
    private float endTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        endTime = Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);
    }

    // Update is called once per frame
    void Update()
    {
        if(!AudioSource.isPlaying)
            timer += Time.deltaTime;

        if(timer >= endTime)
        {
            AudioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);

            endTime = Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);
            timer = 0;
        }
    }
}
