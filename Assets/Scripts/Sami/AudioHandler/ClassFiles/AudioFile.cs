using UnityEngine;

[System.Serializable]
public class AudioEmiterFile
{
    public string name;
    public AudioClip clip;
    public float volume = 1.0f;
    public float pitch = 1.0f;
    public bool loop;
}
