using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public Sprite toggleOn;
    public Sprite toggleOff;
    public GameObject musicToggle;
    public GameObject soundToggle;
    public GameObject menu;
    public GameObject musicAndSound;
    public GameObject about;
    public GameObject tutorial;
    public GameObject page1;
    public GameObject page2;
    public GameObject page3;
    public GameObject page4;
    public GameObject page5;
    public GameObject page6;
    int currentPage;

    void Start()
    {
      currentPage = 1;
      if(PlayerPrefs.GetInt( "Sound", 1 ) == 0)
      {
        soundToggle.GetComponent<Image>().sprite = toggleOff;
      }
      else
      {
        soundToggle.GetComponent<Image>().sprite = toggleOn;
      }
      if(PlayerPrefs.GetInt( "Music", 1 ) == 0)
      {
        musicToggle.GetComponent<Image>().sprite = toggleOff;
      }
      else
      {
        musicToggle.GetComponent<Image>().sprite = toggleOn;
      }
    }

    public void ShowToggles( )
    {
      menu.SetActive( false );
      musicAndSound.SetActive( true );
    }

    public void ShowAbout( )
    {
      menu.SetActive( false );
      about.SetActive( true );
    }

    public void Back( )
    {
      if(menu.activeSelf)
      {
        SceneManager.LoadScene("map");
      }
      else if(about.activeSelf)
      {
        about.SetActive( false );
        menu.SetActive( true );
      }
      else
      {
        musicAndSound.SetActive( false );
        menu.SetActive( true );
      }
    }

    public void ShowTutorial()
    {
      tutorial.SetActive(true);
    }

    public void SoundToggle()
    {
      if(PlayerPrefs.GetInt( "Sound", 1 ) == 0)
      {
        soundToggle.GetComponent<Image>().sprite = toggleOn;
        PlayerPrefs.SetInt( "Sound", 1 );
      }
      else
      {
        soundToggle.GetComponent<Image>().sprite = toggleOff;
        PlayerPrefs.SetInt( "Sound", 0 );
      }
      PlayerPrefs.Save();
      SoundBase.Instance.GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("Sound");
    }

    public void MusicToggle()
    {
      if(PlayerPrefs.GetInt( "Music", 1 ) == 0)
      {
        musicToggle.GetComponent<Image>().sprite = toggleOn;
        PlayerPrefs.SetInt( "Music", 1 );
      }
      else
      {
        musicToggle.GetComponent<Image>().sprite = toggleOff;
        PlayerPrefs.SetInt( "Music", 0 );
      }
      PlayerPrefs.Save();
      GameObject.Find( "Music" ).GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("Music");
    }

    public void NavNext()
    {
      switch(currentPage)
      {
        case 1:
          page1.SetActive(false);
          page2.SetActive(true);
          currentPage += 1;
          break;
        case 2:
          page2.SetActive(false);
          page3.SetActive(true);
          currentPage += 1;
          break;
        case 3:
          page3.SetActive(false);
          page4.SetActive(true);
          currentPage += 1;
          break;
        case 4:
          page4.SetActive(false);
          page5.SetActive(true);
          currentPage += 1;
          break;
        case 5:
          page5.SetActive(false);
          page6.SetActive(true);
          currentPage += 1;
          break;
        case 6:
          ReturnToSettings();
          return;
        default:
          break;
      }
      RawImage curtCircle = (RawImage) GameObject.Find("Circle" + currentPage).GetComponent<RawImage>();
      RawImage prevCircle = (RawImage) GameObject.Find ("Circle" + (currentPage == 1 ? 6 : currentPage - 1)).GetComponent<RawImage>();

      Texture unfilledTexture = prevCircle.texture;
      Texture filledTexture = curtCircle.texture;
      curtCircle.texture = unfilledTexture;
      prevCircle.texture = filledTexture;
    }

    public void NavBack()
    {
      switch(currentPage)
      {
        case 1:
          ReturnToSettings();
          return;
        case 2:
          page2.SetActive(false);
          page1.SetActive(true);
          currentPage -= 1;
          break;
        case 3:
          page3.SetActive(false);
          page2.SetActive(true);
          currentPage -= 1;
          break;
        case 4:
          page4.SetActive(false);
          page3.SetActive(true);
          currentPage -= 1;
          break;
        case 5:
          page5.SetActive(false);
          page4.SetActive(true);
          currentPage -= 1;
          break;
        case 6:
          page6.SetActive(false);
          page5.SetActive(true);
          currentPage -= 1;
          break;
        default:
          break;
      }

      RawImage curtCircle = (RawImage) GameObject.Find("Circle" + currentPage).GetComponent<RawImage>();
      RawImage prevCircle = (RawImage) GameObject.Find ("Circle" + (currentPage + 1)).GetComponent<RawImage>();

      Texture unfilledTexture = prevCircle.texture;
      Texture filledTexture = curtCircle.texture;
      curtCircle.texture = unfilledTexture;
      prevCircle.texture = filledTexture;
    }

    public void ReturnToSettings() {
      page1.SetActive(true);
      page2.SetActive(false);
      page3.SetActive(false);
      page4.SetActive(false);
      page5.SetActive(false);
      page6.SetActive(false);

      if(currentPage > 1) {
        RawImage firstCircle = (RawImage) GameObject.Find("Circle1").GetComponent<RawImage>();
        RawImage currentCircle = (RawImage) GameObject.Find ("Circle" + currentPage).GetComponent<RawImage>();

        Texture unfilledTexture = firstCircle.texture;
        Texture filledTexture = currentCircle.texture;
        currentCircle.texture = unfilledTexture;
        firstCircle.texture = filledTexture;
      }

      tutorial.SetActive(false);
      currentPage = 1;
    }
}
