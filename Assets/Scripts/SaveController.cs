using UnityEngine;

public class SaveController : MonoBehaviour
{
    public void SaveTime(float time)
    {
        PlayerPrefs.SetFloat("Time", time);
    }

    public float LoadTime()
    {
        return PlayerPrefs.GetFloat("Time");
    }

    public void SavePrivacy()
    {
        PlayerPrefs.SetInt("Privacy", 1);
    }

    public bool GetPrivacy()
    {
        if(PlayerPrefs.HasKey("Privacy"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SaveMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("Music Volume", volume);
    }

    public float LoadMusicVolume()
    {
        if(PlayerPrefs.HasKey("Music Volume"))
            return PlayerPrefs.GetFloat("Music Volume");
        else
        {
            return 1;
        }
    }

    public void SaveEffectsVolume(float volume)
    {
        PlayerPrefs.SetFloat("Effects Volume", volume);
    }

    public float LoadEffectsVolume()
    {
        if(PlayerPrefs.HasKey("Effects Volume"))
            return PlayerPrefs.GetFloat("Effects Volume");
        else
        {
            return 1;
        }
    }
}
