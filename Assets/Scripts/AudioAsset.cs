using UnityEngine;

[System.Serializable]
public class AudioAsset
{
    public string key;
    public AudioClip clip;
    [Range(0, 1)] public float volume = 1f; // Defaulted to 1
    [Range(0.5f, 1.5f)] public float pitch = 1f; // Defaulted to 1
    public bool loop = false; // New loop option
}