using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetAudioLevels : MonoBehaviour {

    public GameObject vol_slider_pause;
    public GameObject vol_slider_menu;
    public GameObject effect_slider_pause;
    public GameObject effect_slider_menu;

	public AudioMixer mainMixer;					//Used to hold a reference to the AudioMixer mainMixer


	//Call this function and pass in the float parameter musicLvl to set the volume of the AudioMixerGroup Music in mainMixer
	public void SetMusicLevel(float musicLvl)
	{
		if (musicLvl <= -25){
			mainMixer.SetFloat("musicVol", -80);
		}
		else{
			mainMixer.SetFloat("musicVol", musicLvl);
		}
        GM.gm.music_volume = musicLvl;
        vol_slider_menu.GetComponent<UnityEngine.UI.Slider>().value = GM.gm.music_volume;
        vol_slider_pause.GetComponent<UnityEngine.UI.Slider>().value = GM.gm.music_volume;
	}

	//Call this function and pass in the float parameter sfxLevel to set the volume of the AudioMixerGroup SoundFx in mainMixer
	public void SetSfxLevel(float sfxLevel)
	{
		mainMixer.SetFloat("sfxVol", sfxLevel);
		GM.gm.effect_volume = sfxLevel;
        effect_slider_menu.GetComponent<UnityEngine.UI.Slider>().value = GM.gm.effect_volume;
        effect_slider_pause.GetComponent<UnityEngine.UI.Slider>().value = GM.gm.effect_volume;
	}
}
