using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource; // Referência ao AudioSource

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Verifica se o AudioSource foi configurado corretamente
        if (audioSource == null)
        {
            Debug.LogError("AudioSource não encontrado!");
        }
    }

    void Update()
    {
        // Verifica se a música terminou
        if (!audioSource.isPlaying && audioSource.time > 0)
        {
            // Carrega a cena do menu (substitua "MenuScene" pelo nome da sua cena de menu)
            SceneManager.LoadScene("MainMenu");
        }
    }
}

