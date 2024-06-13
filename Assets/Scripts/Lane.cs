using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public GameObject notePrefab;
    public GameObject points300Prefab;
    public GameObject missPrefab;  // Vari�vel para o missPrefab

    private static GameObject currentPoints300Instance; // Vari�vel para armazenar a inst�ncia atual do points300Prefab (agora � static)
    private static GameObject currentMissInstance; // Vari�vel para armazenar a inst�ncia atual do missPrefab (agora � static)

    private Vector3 fixedPosition; // Posi��o ajust�vel onde os prefabs aparecer�o

    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();

    int spawnIndex = 0;
    int inputIndex = 0;

    void Start()
    {
        // Atualizar a posi��o fixa baseada no tamanho da tela
        UpdateFixedPosition();
    }

    void UpdateFixedPosition()
    {
        // Obter a c�mera principal
        Camera mainCamera = Camera.main;

        // Definir a posi��o fixa no canto inferior esquerdo da tela
        Vector3 screenPosition = new Vector3(0.15f, 0.6f, mainCamera.nearClipPlane + 10f); // Ajuste o 0.05f para posicionar os prefabs adequadamente
        fixedPosition = mainCamera.ViewportToWorldPoint(screenPosition);
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }

    void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (Input.GetKeyDown(input))
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Hit();
                    print($"Hit on {inputIndex} note");
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                }
            }
            if (timeStamp + marginOfError <= audioTime)
            {
                Miss(notes[inputIndex]); // Passar a nota falhada para o m�todo Miss()
                print($"Missed {inputIndex} note");
                inputIndex++;
            }
        }
    }

    private void Hit()
    {
        // Se j� existe uma inst�ncia do points300Prefab, destrua-a
        if (currentPoints300Instance != null)
        {
            Destroy(currentPoints300Instance);
        }

        // Instancie um novo points300Prefab na posi��o fixa e armazene a refer�ncia
        currentPoints300Instance = Instantiate(points300Prefab, fixedPosition, Quaternion.identity);

        // Destrua a inst�ncia atual do missPrefab, se existir
        if (currentMissInstance != null)
        {
            Destroy(currentMissInstance);
        }

        ScoreManager.Hit();
    }

    private void Miss(Note missedNote)
    {
        // Se j� existe uma inst�ncia do missPrefab, destrua-a
        if (currentMissInstance != null)
        {
            Destroy(currentMissInstance);
        }

        // Instancie um novo missPrefab na posi��o fixa e armazene a refer�ncia
        currentMissInstance = Instantiate(missPrefab, fixedPosition, Quaternion.identity);

        // Destrua a inst�ncia atual do points300Prefab, se existir
        if (currentPoints300Instance != null)
        {
            Destroy(currentPoints300Instance);
        }

        // Destrua a nota falhada
        if (missedNote != null)
        {
            Destroy(missedNote.gameObject);
        }

        ScoreManager.Miss();
    }
}
