using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MusicManager : NetworkBehaviour
{
    public WaveSpawner waveSpawner;
    public AudioSource audioSource;
    public AudioClip roundDefeat;
    public AudioClip roundVictory;
    public AudioClip battleFluteFX1;
    public AudioClip battleFluteFX2;
    public AudioClip battleLoopable;
    public AudioClip battleWithIntro;
    public AudioClip preparationLoopable;
    public AudioClip titleScreenLoopable;
    public AudioClip titleScreenWithIntro;

    void Start()
    {
        if (waveSpawner != null)
        {
            waveSpawner.onWaveStart.AddListener(WaveStartMusic);
            waveSpawner.onWaveVictory.AddListener(WaveCompleteVictoryMusic);
            waveSpawner.onWaveDefeat.AddListener(WaveCompleteDefeatMusic);
        }

        audioSource.clip = titleScreenLoopable;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ClientRpc]
    private void WaveStartMusic(){
        audioSource.Stop();
        Debug.Log("Wave Start Music Function Called");
        audioSource.priority = 0;
        audioSource.PlayOneShot(battleFluteFX1);

        audioSource.priority = 128;
        audioSource.clip = battleLoopable;
        audioSource.loop = true;
        audioSource.Play();
    }

    [ClientRpc]
    private void WaveCompleteVictoryMusic(){
        Debug.Log("Wave Complete Victory Music Function Called");
        audioSource.Stop();
        audioSource.priority = 0;
        audioSource.PlayOneShot(roundVictory);

        audioSource.priority = 128;
        audioSource.clip = preparationLoopable;
        audioSource.loop = true;
        audioSource.Play();
    }

    [ClientRpc]
    private void WaveCompleteDefeatMusic(){
        Debug.Log("Wave Complete Defeat Music Function Called");
        audioSource.Stop();
        audioSource.priority = 0;
        audioSource.PlayOneShot(roundDefeat);

        audioSource.priority = 128;
        audioSource.clip = preparationLoopable;
        audioSource.loop = true;
        audioSource.Play();
    }
}
