using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    public AudioSource audioSource;
    [SerializeField]
    List<AudioObject> sounds;

    AudioObject currentSound;

    [SerializeField]
    List<AudioObject> tracks;
    AudioObject currentTrack;
    public void Initialize()
    {
        instance = this;
        Loader.instance.AudioReady = true;
    }
    public static void PlaySound(AudioKey key)
    {
        if (instance != null)
        {
            for (int i = 0; i < instance.sounds.Count; i++)
            {
                if (instance.sounds[i].Key == key)
                {

                    if (!instance.sounds[i].Source.isPlaying)
                    {
                        instance.currentSound = instance.sounds[i];
                        instance.currentSound.Source.volume = instance.sounds[i].Volume;
                        instance.currentSound.Source.Play();
                    }
                }
            }
        }
    }
    public void PlayGameplayTracks()
    {
        int random = Random.Range(1, 3);
        if (currentTrack.Key != tracks[random-1].Key)
        {
            switch (random)
            {
                case 1: PlayTrack(AudioKey.GameplayTrack1); break;
                case 2: PlayTrack(AudioKey.GameplayTrack2); break;
            }
            Invoke("PlayGameplayTracks", currentTrack.Source.clip.length);
        }
        else
        {
            PlayGameplayTracks();
        }
    }
    public static void PlayTrack(AudioKey key)
    {
        for (int i = 0; i < instance.tracks.Count; i++)
        {
            if (instance.tracks[i].Key == key)
            {
                instance.currentTrack = instance.tracks[i];
                instance.currentTrack.Source.volume = instance.tracks[i].Volume;
                instance.currentTrack.Source.Play(1);
                return;
            }
        }
    }
}
[System.Serializable]
public struct AudioObject
{
    [SerializeField]
    AudioKey key;
    public AudioKey Key
    {
        get
        {
            return key;
        }
        set
        {
            key = value;
        }
    }

    [SerializeField]
    AudioSource source;
    public AudioSource Source
    {
        get
        {
            return source;
        }
        set
        {
            source = value;
        }
    }

    [SerializeField]
    float volume;
    public float Volume
    {
        get
        {
            return volume;
        }
        set
        {
            volume = value;
        }
    }

    public void Initialize()
    {
        Volume = Source.volume;
    }

}

public enum AudioKey
{
   Win,Lose, KeyUp, Walk, GameplayTrack1, GameplayTrack2
}