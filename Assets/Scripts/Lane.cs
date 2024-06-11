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
    public GameObject missPrefab;  // Variável para o missPrefab

    private static GameObject currentPoints300Instance; // Variável para armazenar a instância atual do points300Prefab (agora é static)
    private static GameObject currentMissInstance; // Variável para armazenar a instância atual do missPrefab (agora é static)

    private static Vector3 fixedPosition = new Vector3(-8.95f, 0f, 0f); // Posição fixa onde os prefabs aparecerão

    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();

    int spawnIndex = 0;
    int inputIndex = 0;

    void Start()
    {
        // Inicialização, se necessária
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
                Miss();
                print($"Missed {inputIndex} note");
                inputIndex++;
            }
        }
    }

    private void Hit()
    {
        // Se já existe uma instância do points300Prefab, destrua-a
        if (currentPoints300Instance != null)
        {
            Destroy(currentPoints300Instance);
        }

        // Instancie um novo points300Prefab na posição fixa e armazene a referência
        currentPoints300Instance = Instantiate(points300Prefab, fixedPosition, Quaternion.identity);

        // Destrua a instância atual do missPrefab, se existir
        if (currentMissInstance != null)
        {
            Destroy(currentMissInstance);
        }

        ScoreManager.Hit();
    }

    private void Miss()
    {
        // Se já existe uma instância do missPrefab, destrua-a
        if (currentMissInstance != null)
        {
            Destroy(currentMissInstance);
        }

        // Instancie um novo missPrefab na posição fixa e armazene a referência
        currentMissInstance = Instantiate(missPrefab, fixedPosition, Quaternion.identity);

        // Destrua a instância atual do points300Prefab, se existir
        if (currentPoints300Instance != null)
        {
            Destroy(currentPoints300Instance);
        }

        ScoreManager.Miss();
    }
}
