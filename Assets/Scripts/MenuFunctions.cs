using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    private static MenuFunctions _instance;
    public static MenuFunctions Instance
    {
        get
        {
            return _instance;
        }
    }

    private static Dictionary<string, GameObject> menus;
    public List<GameObject> Menus;

    private void Awake()
    {
        if (menus is null)
        {
            menus = new Dictionary<string, GameObject>();
        }
        if (_instance is null)
        {
            _instance = this;
        }
        foreach(GameObject go in Menus)
        {
            menus.Add(go.name, go);
        }
        SceneManager.sceneUnloaded += OnSceneUnload;
    }

    private void OnSceneUnload(Scene scene)
    {
        menus.Clear();
    }

    public static void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public static void StartLevel( string level )
    {
        ResumeGame();
        SceneManager.LoadScene(level);
    }

    private static float preMuteVolume = 100;
    public static void Mute()
    {
        if (AudioListener.volume == 0)
        {
            AudioListener.volume = preMuteVolume;
        }
        else
        {
            preMuteVolume = AudioListener.volume;
            AudioListener.volume = 0;
        }
    }

    public static void SetVolume( float setting )
    {
        AudioListener.volume = setting;
    }

    public static void PauseGame()
    {
        Time.timeScale = 0;
        //AudioListener.pause = true;
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1;
        //AudioListener.pause = false;
    }

    public static void RestartLevel()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowMenu(string menu)
    {
        if (menus.ContainsKey(menu))
        {
            menus[menu].SetActive(true);    
        }
    }

    public void HideMenu(string menu)
    {
        if (menus.ContainsKey(menu))
        {
            menus[menu].SetActive(false);
        }
    }

   public bool IsMenuOpen(string menu)
   {
      if (menus.ContainsKey(menu))
      {
         return menus[menu].activeSelf;
      }
      return false;
   }

}
