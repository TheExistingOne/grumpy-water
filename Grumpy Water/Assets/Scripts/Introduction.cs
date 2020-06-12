using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Introduction : MonoBehaviour
{
    [SerializeField] private String[] script;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image duck;
    [SerializeField] private Image castle;
    [SerializeField] private Image water;
    
    private int _pointer = 0;
    // Start is called before the first frame update
    void Start()
    {
        duck.enabled = false;
        castle.enabled = false;
        water.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_pointer == script.Length - 1)
            {
                SceneManager.LoadScene("Main");
            }
            _pointer++;
        }

        if (_pointer >= 0)
        {
            duck.enabled = true;
        }

        if (_pointer >= 4)
        {
            castle.enabled = true;
        }

        if (_pointer >= 5)
        {
            water.enabled = true;
        }

        text.text = script[_pointer];
    }
}
