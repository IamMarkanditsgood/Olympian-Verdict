using System;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class AudioManager
{
    // Масив для трьох аудіо кліпів
    [SerializeField] private AudioClip[] reincarnationAudioClips;
    [SerializeField] private AudioClip _hellMan;
    [SerializeField] private AudioClip _hellWoman;
    [SerializeField] private AudioClip _heaven;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float delayBetweenTracks = 0f;

    [SerializeField] private string[] _maleNames;
    [SerializeField] private string[] _womanNames;

    public void Stop()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }
    public bool PlayHell(string name)
    {
        /*if (PlayerPrefs.GetInt("Audio") == 1)
        {
            
            for (int i = 0; i < _maleNames.Length; i++)
            {
                Debug.Log("audioSource.clip " + audioSource.clip);
                if (name == _maleNames[i] && audioSource.clip != _hellMan)
                {
                    
                    audioSource.clip = _hellMan;
                    audioSource.Play();
                    
                    return true;
                }
            }
            for (int i = 0; i < _womanNames.Length; i++)
            {
                if (name == _womanNames[i] && audioSource.clip != _hellWoman)
                {
                    audioSource.clip = _hellWoman;
                    audioSource.Play();
                    return false;
                }
            }
            return false;
        }
        return false;*/
        if (audioSource.clip != _hellMan)
        {
            audioSource.clip = _hellMan;
            audioSource.Play();
            
        }
        return true;
    }
    public float GetLenghCurrentSound()
    {
        if (audioSource.isPlaying)
        {
            float timeRemaining = audioSource.clip.length - audioSource.time;
            return timeRemaining;
        }
        return 0;
    }
    public void PlayHeaven()
    {
        if (PlayerPrefs.GetInt("Audio") == 1)
        {
            if (audioSource.clip != _heaven)
            {
                Debug.Log(audioSource);
                Debug.Log(_heaven);
                audioSource.clip = _heaven;
                audioSource.Play();
            }
        }
    }
    public async void ReincarnatePlay()
    {
        if (PlayerPrefs.GetInt("Audio") == 1)
        {
            await PlaySoundsReincarnation();
        }
    }

    private async Task PlaySoundsReincarnation()
    {
        if (audioSource.clip != reincarnationAudioClips[0])
        {
            foreach (AudioClip clip in reincarnationAudioClips)
            {
                audioSource.clip = clip;
                audioSource.Play();

                await Task.Delay((int)((clip.length + delayBetweenTracks) * 1000));
            }
        }
    }
}
