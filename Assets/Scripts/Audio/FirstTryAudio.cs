using DeathCount;
using UnityEngine;

public class FirstTryAudio : MonoBehaviour
{
    public AudioSource fristTryAudio;
    public AudioSource under7Times;
    public AudioSource over7Times;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (DeathCounter.instance != null)
        {
            if (DeathCounter.instance.deathCount == 0)
            {
                fristTryAudio.Play();
            }
            else if (DeathCounter.instance.deathCount <= 7)
            {
                under7Times.Play();
            }
            else if (DeathCounter.instance.deathCount >= 8)
            {
                over7Times.Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (DeathCounter.instance != null)
        // {
        //     if (DeathCounter.instance.deathCount == 0)
        //     {
        //         voiceOver.Play();
        //     }
        // }
    }
}
