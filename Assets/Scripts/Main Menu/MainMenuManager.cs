using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Canvas")]
    public Canvas main;
    public Canvas instructions;
    public Canvas credits;
    private List<Canvas> canvas; 

    [Header("Instructions")]
    public TextMeshProUGUI instructionText;
    public List<string> instructionList;
    private int activeInstruction = 0;

    [Header("Music Toggling")]
    public GameManager gameManager;
    public Toggle toggle;

    void Awake()
    {
        canvas = new List<Canvas>();
        canvas.Add(main);
        canvas.Add(instructions);
        canvas.Add(credits);    
        SetCanvas(main);
        UpdateInstructions();
        gameManager = FindFirstObjectByType<GameManager>();
    }

   public void Play()
    {
        SceneManager.LoadScene("Lobby");
        if (!gameManager.GetDayInProgress())
        {
            gameManager.SetDayInProgress(true);
            StartCoroutine(gameManager.DayCountdown());
        }
    }
        
    public void Instructions()
    {
        SetCanvas(instructions);
    }

    public void Credits()
    {
        SetCanvas(credits);
    }
    
    public void Back()
    {
        SetCanvas(main);
    }

    public void UpdateInstructions()
    {
        instructionText.text = instructionList[activeInstruction];
    }

    public void Next()
    {
        if (activeInstruction < instructionList.Count - 1) activeInstruction++;
        UpdateInstructions();
    }

    public void Prev()
    {
        if (activeInstruction > 0) activeInstruction--;
        UpdateInstructions();
    }

    public void SetCanvas(Canvas c)
    {
        for (int i = 0; i < canvas.Count; i++)
        {
            if (c == canvas[i]) canvas[i].enabled = true;
            else canvas[i].enabled = false;
        }
    }

    public void ToggleMusic()
    {
        gameManager.SetPlayMusic(toggle.isOn);
    }
}
