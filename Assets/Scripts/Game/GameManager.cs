using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // GameManager
    public static GameManager Instance { get; private set; }

    [Header("Default Player")]
    // Player Stats
    private int money = 100;
    public int startingDebt = 200;
    private int debt;
    private int day = 0;

    // Upgrades
    private bool hasDoorbell = false;
    private int postersBought = 0;
    private bool hasStainlessPan = false;

    // Day data
    private int tips = 0; // resets at end of day
    private int satisfaction = 0;
    private int bonus = 25; // based on satisfaction built up over the week
    private int customersServed = 0;
    private int irsSpotted = 0;
    private bool dayInProgress = false;
    
    [Header("Audio")]
    public AudioSource music;
    public AudioSource sfx;

    // Music
    private bool playMusic = true;
    private AudioClip activeSong;
    public AudioClip titleMus;
    public AudioClip kitchenMus;
    public AudioClip computerMus;

    // SFX
    public AudioClip sizzle;

    private void Awake()
    {
        debt = startingDebt;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public void Update()
    {
        // Music
        if (playMusic)
        {
            string scene = SceneManager.GetActiveScene().name;
            AudioClip musToPlay = null;
            switch (scene)
            {
                case "Main Menu":
                    musToPlay = titleMus;
                    break;
                case "Computer Menu":
                    musToPlay = computerMus;
                    break;
                default:
                    musToPlay = kitchenMus;
                    break;
            }
            if (musToPlay != null && activeSong != musToPlay)
            {
                PlaySong(musToPlay);
            }
        }
        else if (activeSong != null)
        {
            music.Stop();
            activeSong = null;
        }

    }

    public void PlaySong(AudioClip song)
    {
        music.Stop();
        music.clip = song;
        activeSong = song;
        music.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        sfx.PlayOneShot(clip);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void SetPlayMusic(bool b) { playMusic = b; }

    public void AddMoney(int i) { money += i; }

    public void SubtractMoney(int i) { money -= i; }

    public void PayDebt(int i)
    {
        SubtractMoney(i);
        debt -= i;
    }

    public int GetPaidDebt() { return startingDebt - debt; }

    public void UnlockUpgrade(string upgrade)
    {
        switch (upgrade)
        {
            case "Doorbell":
                hasDoorbell = true;
                break;
            case "Poster":
                postersBought++;
                break;
            case "Stainless Pan":
                hasStainlessPan = true;
                break;
        }
    }

    public IEnumerator DayCountdown()
    {
        day++;
        tips = 0;
        satisfaction = 0;
        customersServed = 0;
        irsSpotted = 0;
        Debug.Log("starting day" + day);
        yield return new WaitForSeconds(2f);
        dayInProgress = false;
        Debug.Log(dayInProgress);
    }

    // Getters for private variables
    public int GetMoney() { return money; }
    public int GetDebt() { return debt; }
    public int GetDay() { return day; }

    public bool GetHasDoorbell() { return hasDoorbell; }
    public int GetPostersBought() { return postersBought; }
    public bool GetHasStainlessPan() { return hasStainlessPan; }

    public int GetTips() { return tips; }
    public int GetSatisfaction() { return satisfaction; }
    public int GetBonus() { return bonus; }
    public int GetCustomersServed() { return customersServed; }
    public int GetIrsSpotted() { return irsSpotted; }
    public bool GetDayInProgress() { return dayInProgress; }
    public void SetDayInProgress(bool b) { dayInProgress = b; }

    public bool GetPlayMusic() { return playMusic; }
}
