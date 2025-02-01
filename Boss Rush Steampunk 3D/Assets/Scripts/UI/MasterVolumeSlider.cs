using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterVolumeSlider : MonoBehaviour
{
    public AudioMixerGroup masterGroup;
    public static AudioMixerGroup master;
    private Slider volumeSlider;
    public float maxDB = 5, minDB = -80;
    private float maxSlide, minSlide, rangeSlide, rangeDB;
    public static float masterVolume;

    private void Start()
    {
        master = masterGroup;
        volumeSlider = GetComponent<Slider>();
        maxSlide = volumeSlider.maxValue;
        minSlide = volumeSlider.minValue;
        rangeSlide = maxSlide - minSlide;
        rangeDB = maxDB - minDB;
    }

    void Update()
    {
        //  Do decibel conversions and stuff to make the slider nice. Thank you Desmos https://www.desmos.com/calculator/r6ifpdtgeb
        float x = volumeSlider.value;
        float a = rangeDB / Mathf.Log10(rangeSlide + 1);
        masterVolume = a * Mathf.Log10(x - minSlide + 1) + minDB;
    }

    //  Called constantly by the cursor object
    public static void SetVolume()
    {
        master.audioMixer.SetFloat("masterVolume", masterVolume);
    }
}
