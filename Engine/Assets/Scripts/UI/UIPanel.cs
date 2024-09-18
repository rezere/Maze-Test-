using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText, keyText;



    private void Start()
    {
        GameController.instance.KeyPicked += UpdateKey;
        Initialized();
    }

    private void Initialized()
    {
        keyText.text =  "0/" + GameController.instance.CountKey;
        timerText.text = FormatTime(GameController.instance.TimeGame);
    }
    private void OnDisable()
    {
        GameController.instance.KeyPicked -= UpdateKey;
    }
    private void Update()
    {
        timerText.text = FormatTime(GameController.instance.TimeGame);
    }
    private void UpdateKey()
    {
        keyText.text = GameController.instance.CurrentKey.ToString() + "/" + GameController.instance.CountKey;
    }
    private string FormatTime(int time)
    {
        int minutes = time / 60;
        int seconds = time % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
