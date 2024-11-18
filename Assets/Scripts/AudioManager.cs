using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    
    public static AudioManager instance { get; private set; }
    
    [Header("Volume / Utility")]
    [SerializeField] private float SFXVolume = 1f;
    [SerializeField] private float BGMVolume = 1f;
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private string currentPerspective = "top";

    [Header("Clips")]
    [SerializeField] private AudioClip footstep;
    [SerializeField] private AudioClip rock;
    [SerializeField] private AudioClip lever;
    [SerializeField] private AudioClip button;

    [Header("Music")]
    [SerializeField] private AudioClip firstPersonBGM;
    [SerializeField] private AudioClip topDownBGM;

    [Header("Noise")]
    [SerializeField] private AudioClip firstPersonBGN;
    [SerializeField] private AudioClip topDownBGN;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource asSFX;
    [SerializeField] private AudioSource asFootStep;
    [SerializeField] private AudioSource asFirstPersonBGM;
    [SerializeField] private AudioSource asTopDownBGM;
    [SerializeField] private AudioSource asFirstPersonBGN;
    [SerializeField] private AudioSource asTopDownBGN;



    private void Awake()  {         
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        } 
    }

    void Start() {

        DontDestroyOnLoad(this);

        asFootStep.clip = footstep;

        // Assign music.
        asFirstPersonBGM.clip = firstPersonBGM;
        asTopDownBGM.clip = topDownBGM;

        // Assign noise.
        asFirstPersonBGN.clip = firstPersonBGN;
        asTopDownBGN.clip = topDownBGN;

        asTopDownBGN.Play();
        asFirstPersonBGN.Play();

        SwitchMusic(currentPerspective);
    }

    public void PlayFootStep() {
        if (!asFootStep.isPlaying) {
            asFootStep.pitch = 0.45f + Random.Range(-0.05f, 0.05f);
            asFootStep.Play();
        }
    }

    public void PlayRockPush() {
        asSFX.clip = rock;
        asSFX.Play();
    }

    public void PlayButton() {
        asSFX.clip = button;
        asSFX.Play();
    }

    public void PlayLever() {
        asSFX.clip = lever;
        asSFX.Play();
    }

    public void PlayMusic() {

        if (!asTopDownBGM.isPlaying) {
            asTopDownBGM.Play();
            asFirstPersonBGM.Play();
        }

        if (currentPerspective == "top") {
            asTopDownBGM.volume = 1f;
            asFirstPersonBGM.volume = 0f;

            asTopDownBGN.volume = 1f;
            asFirstPersonBGN.volume = 0f;
        } else {
            asTopDownBGM.volume = 0f;
            asFirstPersonBGM.volume = 1f;

            asTopDownBGN.volume = 0f;
            asFirstPersonBGN.volume = 1f;
        }
    }

    public void StopMusic() {
        asTopDownBGM.Stop();
        asFirstPersonBGM.Stop();
    }

    public void SwitchMusic(string perspective) {
        currentPerspective = perspective;
        switch (perspective) {
            case "first":
                StartCoroutine(Crossfade(asTopDownBGM, asFirstPersonBGM));
                StartCoroutine(Crossfade(asTopDownBGN, asFirstPersonBGN));
                break;
            case "top":
                StartCoroutine(Crossfade(asFirstPersonBGM, asTopDownBGM));
                StartCoroutine(Crossfade(asFirstPersonBGN, asTopDownBGN));
                break;
        }
    }

    public IEnumerator Crossfade(AudioSource toBeLowered, AudioSource toBeRaised) {
        float elapsedTime = 0f;

        // Store initial volumes
        float startVolumeLower = toBeLowered.volume;
        float startVolumeRaise = toBeRaised.volume;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;

            // Interpolate volumes
            toBeLowered.volume = Mathf.Lerp(startVolumeLower, 0f, t);
            toBeRaised.volume = Mathf.Lerp(startVolumeRaise, BGMVolume, t);

            yield return null;
        }

        // Ensure final values are set
        toBeLowered.volume = 0f;
        toBeRaised.volume = BGMVolume;
    }

}
