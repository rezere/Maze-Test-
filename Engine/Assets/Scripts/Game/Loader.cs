using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public static Loader instance;
    [SerializeField] private GameController gameController;
    private bool gameReady;
    public bool GameReady
    {
        get { return gameReady; }
        set 
        { 
            gameReady = value;
            Ready();
        }
    }
    [SerializeField] private AudioController audioController;
    private bool audioReady;
    public bool AudioReady
    {
        get { return audioReady; }
        set 
        { 
            audioReady = value;
            Ready();
        }
    }

    private void Awake()
    {
        instance = this;
        gameController.Initialize();
        audioController.Initialize();
    }

    private void Ready()
    {
        if (AudioReady && GameReady)
        { 
            GameController.instance.StartGame();
            AudioController.instance.PlayGameplayTracks();
        }
    }
    
}
