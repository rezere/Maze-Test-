using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance; 
    [SerializeField] private int countKey, currentKey;
    [SerializeField] private float mazeWidth, mazeLength;
    [SerializeField] private List<Transform> spawnPos;
    [SerializeField] private Vector3 finishMoveText, finishSizeText;

    public int CountKey { get { return countKey; } }
    public int CurrentKey
    {
        get { return currentKey; }
        set
        {
            currentKey += value;
            KeyPicked?.Invoke();
            if (value != 0)
            {
                scoreText.gameObject.SetActive(true);
                StartCoroutine(MoveToTarge(finishMoveText, 2f, finishSizeText, finishSizeText));
            }
        }

    }
    public event Action KeyPicked;
    private int timeGame;
    private bool isPause;
    public bool IsPaused
    { 
        get { return isPause; } 
        set 
        { 
            isPause = value;
            pauseWindow?.SetActive(value);
            InfoText = !value;
            if (!isPause) Cursor.lockState = CursorLockMode.Locked;
            else Cursor.lockState = CursorLockMode.None;
        }   
    }
    public int TimeGame { get { return timeGame; } }

    [Header("UI")]
    [SerializeField] private GameObject pauseWindow, endWindow;
    [SerializeField] private TMP_Text endText, scoreText, infoText;
    public bool InfoText 
    {
        get { return infoText; }
        set 
        {
            infoText.gameObject.SetActive(value);
        }    
    } 
    [Header("Prefabs")]
    [SerializeField] private GameObject keyPrefab;
    private void Awake()
    {
        instance = this;
    }

    private void OnDisable()
    {
    }

    public void Initialize()
    {
        instance = this;
        timeGame = 0;
        CurrentKey = 0;
        IsPaused = false;
        scoreText.transform.localPosition = Vector3.zero;
        SpawnKeys();
        Loader.instance.GameReady = true;
    }
    public void StartGame()
    {
        StartCoroutine(GameTimer());
    }
    public bool DoorOpen()
    {
       return countKey == CurrentKey;
    }
    public void EndGame(bool isFinish)
    {
        if (isFinish)
        {
            endText.text = "Перемога";
            AudioController.PlaySound(AudioKey.Win);
        }
        else
        {
            endText.text = "Програш";
            AudioController.PlaySound(AudioKey.Lose);
        }
        endWindow?.SetActive(true);
        IsPaused = true;
        StopAllCoroutines();
    }
    private IEnumerator GameTimer()
    {

        while (true)
        {
            if(!IsPaused) timeGame++;

            yield return new WaitForSeconds(1f);
        }
    }

    public void ContinueGame()
    {
        IsPaused = false;
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void SpawnKeys()
    {
        List<int> pos = new List<int>();
        int tempPos;
        for (int i = 0; i < countKey; i++)
        {
            tempPos = Random.Range(0, spawnPos.Count+1);
            if (!pos.Contains(tempPos))
            {
                pos.Add(tempPos);
                Instantiate(keyPrefab, spawnPos[tempPos].position, Quaternion.identity);
            }
            else
            {
                i--;
            }
        }
    }
    private IEnumerator MoveToTarge(Vector3 targetPosition, float time, Vector3 finishScale, Vector2 finishSize)
    {
        Vector3 tempBasePosition = scoreText.transform.localPosition;

        Vector3 tempBaseScale = scoreText.transform.localScale;

        float currentMoveTime = 0;

        yield return new WaitForSeconds(0.15f);

        while (Vector3.Distance(scoreText.transform.localPosition, targetPosition) > 0.1f)
        {
            scoreText.transform.localPosition = Vector3.Lerp(tempBasePosition, targetPosition, (currentMoveTime / time));

            scoreText.transform.localScale = Vector3.Lerp(tempBaseScale, finishScale, (currentMoveTime / time));
            currentMoveTime += Time.deltaTime;

            yield return null;
        }
        scoreText.gameObject.SetActive(false);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
