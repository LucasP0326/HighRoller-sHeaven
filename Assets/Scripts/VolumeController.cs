using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume") && PlayerPrefs.HasKey("MusicVolume") && PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadVolume("MasterVolume", "MusicVolume", "SFXVolume");
        }
        else
        {
            // Set default values if no saved preferences found
            SetMasterVolume(0.75f); // Default Master volume
            SetMusicVolume(0.5f);   // Default Music volume
            SetSFXVolume(0.5f);     // Default SFX volume
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterSlider.value = volume;
        mixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicSlider.value = volume;
        mixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSlider.value = volume;
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume(string masterKey, string musicKey, string sfxKey)
    {
        SetMasterVolume(PlayerPrefs.GetFloat(masterKey, 0.75f)); // Default Master volume
        SetMusicVolume(PlayerPrefs.GetFloat(musicKey, 0.5f));     // Default Music volume
        SetSFXVolume(PlayerPrefs.GetFloat(sfxKey, 0.5f));         // Default SFX volume
    }
}