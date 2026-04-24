using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectHandler : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> m_generators;

    [SerializeField]
    private Vector2 m_minMaxPitch;

    [SerializeField]
    private List<AudioSource> m_audioSource;

    public void Play()
    {
        AudioSource source = m_audioSource[0];

        if (!source.isPlaying)
        {
            source = m_audioSource[1];
        }

        int index = UnityEngine.Random.Range(0, m_generators.Count);
        AudioClip generator = m_generators[index];
        source.generator = generator;
        source.pitch = Random.Range(m_minMaxPitch.x, m_minMaxPitch.y);
        source.Play();
    }
}
