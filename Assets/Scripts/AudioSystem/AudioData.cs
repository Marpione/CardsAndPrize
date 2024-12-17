using System.Collections.Generic;
using UnityEngine;

public class AudioData : ScriptableObject
{
    [SerializeField] private List<string> _keys = new();
    [SerializeField] private List<AudioClip> _values = new();

    public Dictionary<string, AudioClip> GetAudioDictionary()
    {
        var dict = new Dictionary<string, AudioClip>();
        for (int i = 0; i < Mathf.Min(_keys.Count, _values.Count); i++)
        {
            if (!string.IsNullOrEmpty(_keys[i]) && _values[i] != null)
                dict.Add(_keys[i], _values[i]);
        }
        return dict;
    }
}
