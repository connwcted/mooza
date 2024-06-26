using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;
    public Button helpButton;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("LevelSelect");
        });

        helpButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Help");
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }


}