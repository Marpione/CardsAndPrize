using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioData _audioData;
    [SerializeField] private StringEventChannel _audioEvent;
    private Dictionary<string, AudioClip> _audioClips;

    private void Awake()
    {
        _audioClips = _audioData.GetAudioDictionary();
    }

    private void OnEnable() => _audioEvent.OnEventRaised += PlayAudio;
    private void OnDisable() => _audioEvent.OnEventRaised -= PlayAudio;

    private void PlayAudio(string audioId)
    {
        if (_audioClips.TryGetValue(audioId, out AudioClip clip))
        {
            var audioObject = new GameObject($"Audio_{audioId}");
            var audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();

            float clipLength = clip.length;
            Destroy(audioObject, clipLength);
        }
    }
}