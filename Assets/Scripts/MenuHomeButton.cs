using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeMenuButton : MonoBehaviour
{
    public Button homeButton;

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
        homeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("LevelSelect");
        });
    }
}
