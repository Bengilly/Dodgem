using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    public float volumeValue;

    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private Text volumeText = null;
    [SerializeField] private GameManager gameManager;

    public void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }
    private void Start()
    {
        
    }

    private void Update()
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

        //update audio source volume
        gameManager.UpdateVolume();
    }

    public float GetVolume()
    {
        volumeValue = PlayerPrefs.GetFloat("VolumeValue", volumeValue);
        volumeText.text = volumeValue.ToString();
        volumeSlider.value = volumeValue;
        return volumeValue/10;
    }
    
    public void SaveVolume(float volume)
    {
        PlayerPrefs.SetFloat("VolumeValue", volumeValue);
        volumeValue = volume;
    }
}
