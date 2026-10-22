using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Crater Coin Rush - one GameManager per level (Level1, Level2).
// Counts the coins in the scene, runs the countdown timer, updates the HUD,
// loads the next scene when every coin is collected and restarts the level if time runs out.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Total time across both levels, shown on the Mission Complete screen
    public static float TotalTimeTaken;

    [Header("HUD")]
    public TMP_Text coinText;        // "Coins: 3 / 12"
    public TMP_Text timerText;       // "2:41"
    public TMP_Text messageText;     // pop-up messages

    [Header("Level settings")]
    public string levelName = "Level 1";
    [Tooltip("Seconds the player has to collect every coin.")]
    public float timeLimit = 180f;
    [Tooltip("Scene to load when all coins are collected (Level2 or MissionComplete).")]
    public string nextSceneName = "Level2";
    [Tooltip("Tick on Level 1 so the total time starts from zero.")]
    public bool isFirstLevel = true;
    public float delayBeforeNextScene = 2.5f;

    [Header("Optional sounds")]
    public AudioClip levelCompleteSound;
    public AudioClip timeUpSound;

    private int collected;
    private int total;
    private float timeLeft;
    private bool levelOver;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (isFirstLevel) TotalTimeTaken = 0f;

        total = FindObjectsByType<Collectible>(FindObjectsSortMode.None).Length;
        timeLeft = timeLimit;
        UpdateHUD();
        ShowMessage(levelName + ": collect all " + total + " coins!", 3f);
    }

    private void Update()
    {
        if (levelOver) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            TimeUp();
        }
        UpdateTimer();
    }

    public void AddScore(int amount)
    {
        if (levelOver) return;
        collected += amount;
        UpdateHUD();

        if (collected >= total) LevelComplete();
    }

    private void LevelComplete()
    {
        levelOver = true;
        TotalTimeTaken += timeLimit - timeLeft;
        if (levelCompleteSound) AudioSource.PlayClipAtPoint(levelCompleteSound, Camera.main.transform.position);
        ShowMessage("All coins collected!", delayBeforeNextScene);
        Invoke(nameof(LoadNext), delayBeforeNextScene);
    }

    private void TimeUp()
    {
        levelOver = true;
        if (timeUpSound) AudioSource.PlayClipAtPoint(timeUpSound, Camera.main.transform.position);
        ShowMessage("Time's up! Restarting...", delayBeforeNextScene);
        Invoke(nameof(RestartLevel), delayBeforeNextScene);
    }

    private void UpdateHUD()
    {
        if (coinText) coinText.text = "Coins: " + collected + " / " + total;
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if (!timerText) return;
        int mins = Mathf.FloorToInt(timeLeft / 60f);
        int secs = Mathf.FloorToInt(timeLeft % 60f);
        timerText.text = mins + ":" + secs.ToString("00");
        // Turn red in the last 30 seconds
        timerText.color = timeLeft <= 30f ? Color.red : Color.white;
    }

    public void ShowMessage(string msg, float seconds)
    {
        if (!messageText) return;
        messageText.text = msg;
        messageText.gameObject.SetActive(true);
        CancelInvoke(nameof(HideMessage));
        Invoke(nameof(HideMessage), seconds);
    }

    private void HideMessage()
    {
        if (messageText) messageText.gameObject.SetActive(false);
    }

    private void LoadNext()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
