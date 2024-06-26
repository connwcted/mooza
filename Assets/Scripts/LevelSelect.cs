using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelSelect : MonoBehaviour
{
    public Button FrostLandButton;
    public Button PhotoButton;
    public Button VinxisButton;
    public Button BackButton;

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
        FrostLandButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("FrostLand");
        });

        PhotoButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Photo");
        });

        VinxisButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Vinxis");
        });

        BackButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainMenu");
        });
    }
}
