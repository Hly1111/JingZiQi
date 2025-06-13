using System.Collections;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip[] bgmClips;
    private int _currentIndex = 0;
    private readonly bool _looping = true;

    public void PlaySound(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    private void Start()
    {
        if (bgmClips.Length == 0) return;
        for (int i = 0; i < bgmClips.Length; i++)
        {
            int r = Random.Range(i, bgmClips.Length);
            (bgmClips[i], bgmClips[r]) = (bgmClips[r], bgmClips[i]);
        }

        StartCoroutine(PlayBGMList());
    }

    private IEnumerator PlayBGMList()
    {
        while (true)
        {
            audioSource.clip = bgmClips[_currentIndex];
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length + 1f);
            _currentIndex = (_currentIndex + 1) % bgmClips.Length;
            if (!_looping && _currentIndex == 0)
            {
                yield break;
            }
        }
    }
}
