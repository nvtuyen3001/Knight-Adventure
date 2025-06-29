using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("volume")){
            Load();
        }
        else{
            volumeSlider.value = PlayerPrefs.GetFloat("volume");
        }
    }

    // Update is called once per frame
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load(){
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }

    private void Save(){   
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }
}
