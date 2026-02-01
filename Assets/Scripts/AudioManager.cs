using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioAsset[] library;
    private Dictionary<string, AudioAsset> audioTable = new Dictionary<string, AudioAsset>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var asset in library)
        {
            if (!audioTable.ContainsKey(asset.key))
                audioTable.Add(asset.key, asset);
        }
    }

    public void PlaySound(string key, AudioSource emitter)
    {
        if (audioTable.TryGetValue(key, out AudioAsset asset))
        {
            emitter.clip = asset.clip;
            emitter.volume = asset.volume;
            emitter.pitch = asset.pitch;
            emitter.loop = asset.loop;
            emitter.Play();
        }
        else
        {
            Debug.LogWarning($"Sound: {key} not found!");
        }
    }

    // Immediately stop the sound
    public void StopSound(AudioSource emitter)
    {
        emitter.Stop();
    }

    // Useful for smooth transitions (e.g., stopping music on death)
    public void FadeOutSound(AudioSource emitter, float duration)
    {
        StartCoroutine(FadeOutCoroutine(emitter, duration));
    }

    private IEnumerator FadeOutCoroutine(AudioSource emitter, float duration)
    {
        float startVolume = emitter.volume;

        while (emitter.volume > 0)
        {
            emitter.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        emitter.Stop();
        emitter.volume = startVolume;
    }
}