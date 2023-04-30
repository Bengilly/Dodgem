using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private Text volumeText = null;
    public float volumeValue;

    private void Start()
    {
        
    }

    //run each time the volume slider is moved
    public void VolumeSlider(float volume)
    {
        //round up volume
        volume = Mathf.Round(volume);
        volumeText.text = volume.ToString("0");
        volumeValue = volume;
        volumeSlider.value = volumeValue;
    }

    public float GetVolume()
    {
        volumeValue = PlayerPrefs.GetFloat("VolumeValue", volumeValue);
        volumeText.text = volumeValue.ToString("0");
        volumeSlider.value = volumeValue;
        return volumeValue;
    }

    public void SaveVolume(float volume)
    {
        PlayerPrefs.SetFloat("VolumeValue", volumeValue);
        volumeValue = volume;
    }
}
