using UnityEngine;
using UnityEngine.UI;

public enum GameStates
{
    MAINMENU,
    GAMEPLAY,
    SETTINGS,
    PRIVACY,
    SCORE
}

public class GameController : MonoBehaviour
{
    private GameStates currentGameState;

    public GameObject ui_MainMenu;
    public GameObject ui_Gameplay;
    public GameObject ui_Settings;
    public GameObject ui_Score;
    public GameObject ui_Privacy;

    public float time = 0;
    public float bestTime = 0;

    public Text timeText;
    public Text bestTimeText;
    public Text actualTime;

    public GameObject spawner;

    public SaveController saveManager;
    public AdBehaviour adManager;

    // Start is called before the first frame update
    void Start()
    {
        saveManager = FindObjectOfType(typeof(SaveController)) as SaveController;
        adManager = FindObjectOfType(typeof(AdBehaviour)) as AdBehaviour;
        bestTime = saveManager.LoadTime();

        //FIRST TIME PLAYING, SHOW PRIVACY CONTENT
        if (saveManager.GetPrivacy())
        {
            CallMainMenu_UI();
        }
        else
        {
            CallPrivacy_UI();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Finite-State Machine
        switch (currentGameState)
        {
            case GameStates.MAINMENU:
            {
                ui_MainMenu.SetActive(true);
                ui_Gameplay.SetActive(false);
                ui_Settings.SetActive(false);
                ui_Score.SetActive(false);
                ui_Privacy.SetActive(false);
            }
            break;

            case GameStates.GAMEPLAY:
            {
                ui_MainMenu.SetActive(false);
                ui_Gameplay.SetActive(true);
                ui_Settings.SetActive(false);
                ui_Score.SetActive(false);
                ui_Privacy.SetActive(false);

                time += Time.deltaTime;

                if(time > 99999)
                {
                    time = 99999;
                }

                timeText.text = time.ToString("0.00");

            }
            break;

            case GameStates.PRIVACY:
            {
                ui_MainMenu.SetActive(false);
                ui_Gameplay.SetActive(false);
                ui_Settings.SetActive(false);
                ui_Score.SetActive(false);
                ui_Privacy.SetActive(true);
            }
            break;

            case GameStates.SETTINGS:
            {
                ui_MainMenu.SetActive(false);
                ui_Gameplay.SetActive(false);
                ui_Score.SetActive(false);
                ui_Privacy.SetActive(false);
                ui_Settings.SetActive(true);
            }
            break;

            case GameStates.SCORE:
            {
                ui_MainMenu.SetActive(false);
                ui_Gameplay.SetActive(false);
                ui_Settings.SetActive(false);
                ui_Score.SetActive(true);
                ui_Privacy.SetActive(false);
            }
            break;
        }
    }

    public void CallMainMenu_UI()
    {
        currentGameState = GameStates.MAINMENU;
    }

    public void CallGameplay_UI()
    {
        currentGameState = GameStates.GAMEPLAY;
    }

    public void CallSettings_UI()
    {
        currentGameState = GameStates.SETTINGS;
    }

    public void CallPrivacy_UI()
    {
        currentGameState = GameStates.PRIVACY;
    }

    public void CallScore_UI()
    {
        bestTimeText.text = bestTime.ToString("0.00");
        actualTime.text = time.ToString("0.00");
        currentGameState = GameStates.SCORE;
    }

    public void Accept()
    {
        saveManager.SavePrivacy();
        CallMainMenu_UI();
    }

    public void CallGameOver()
    {
        if (adManager)
        {
            adManager.GameOver();
        }
        
        EnemyBehaviour[] remainingEnemies = FindObjectsOfType(typeof(EnemyBehaviour)) as EnemyBehaviour[];

        foreach(EnemyBehaviour e in remainingEnemies)
        {
            e.dead = false;
            e.material.SetFloat("_Fade", 1);
            e.gameObject.SetActive(false);
        }

        if(time > bestTime)
        {
            saveManager.SaveTime(time);
            bestTime = saveManager.LoadTime();
        }
        else
        {
            bestTime = saveManager.LoadTime();
        }

        CallScore_UI();
        time = 0;
    }

    public GameStates GetCurrentGameState()
    {
        return currentGameState;
    }

    public void ButtonEffect()
    {
        AudioController.PlaySound(SoundEffects.BUTTON);
    }

    
}
