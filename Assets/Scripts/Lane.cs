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

    private static Vector3 fixedPosition = new Vector3(-8.95f, 0f, 0f); // Posi��o fixa onde os prefabs aparecer�o

    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();

    int spawnIndex = 0;
    int inputIndex = 0;

    void Start()
    {
        // Inicializa��o, se necess�ria
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

    private void Miss()
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

        ScoreManager.Miss();
    }
}
