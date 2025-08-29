using System.Collections.Generic;
using UnityEngine;


//Tässä Scriptissä AudioSource On Pakattu Samaan Pakettiin 
public class AudioEmiter : MonoBehaviour
{
    [SerializeField] private List<AudioEmiterFile> m_AudioClip;
    private AudioSource audioHandler;
    [SerializeField] private string playThisOnGameStart;

    private void Start()
    {
        audioHandler = gameObject.AddComponent<AudioSource>();
        if (playThisOnGameStart != "")
        {
            EmitSound(playThisOnGameStart);
        }
    }

    public void EmitSound(string name)
    {
        foreach (AudioEmiterFile file in m_AudioClip)
        {
            if (file.name == name)
            {
                audioHandler.clip = file.clip;
                audioHandler.volume = file.volume;
                audioHandler.pitch = file.pitch;
                audioHandler.loop = file.loop;
                audioHandler.Play();
                break;
            }
        }
    }
    public void PlayByID(int id)
    {
        audioHandler.clip = m_AudioClip[id].clip;
        audioHandler.volume = m_AudioClip[id].volume;
        audioHandler.pitch = m_AudioClip[id].pitch;
        audioHandler.Play();
    }
    public void StopSound()
    {
        audioHandler.Stop();
    }
}