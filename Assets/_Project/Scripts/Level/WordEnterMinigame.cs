using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class TypingMinigame : MonoBehaviour, ICompletable
{
    [Header("UI References")]
    [SerializeField] private TMP_Text wordDisplay;
    [SerializeField] private TMP_Text inputDisplay;
    [SerializeField] private TMP_Text streakDisplay;
    [SerializeField] private TMP_Text startPrompt;
    [SerializeField] private Image timerBar;
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private Transform playerSpot;
    private const int MAX_INPUT_LENGTH = 5;
    [Header("Settings")]
    [SerializeField] private float timeLimit = 10f;
    [SerializeField] private string[] wordPool;
    [SerializeField] private int wordsPerSession = 5;
    [SerializeField] private int streakToWin = 3;

    [Inject] private Player player;
    private bool isOpened = false;
    private string currentWord;
    private string playerInput = "";
    private int correctInRow = 0;
    private int wordsTyped = 0;
    private float timeLeft;
    private bool isActive = false;
    private bool hasStarted = false;
    private bool hasWon = false;

    public event System.Action Completed;

    private void Update()
    {
        if (!isOpened)
            return;

        if (isOpened && !hasStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartRealGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopMinigame();
            return;
        }

        if (hasWon && Input.GetKeyDown(KeyCode.Space))
        {
            StartRealGame();
            return;
        }

        if (!isActive) return;

        // Timer update
        timeLeft -= Time.deltaTime;
        timerBar.fillAmount = Mathf.Clamp01(timeLeft / timeLimit);

        if (timeLeft <= 0f)
        {
            correctInRow = 0;
            wordsTyped++;
            if (wordsTyped >= wordsPerSession)
            {
                EndSession();
                return;
            }
            NextWord();
        }

        // Handle typing
        foreach (char c in Input.inputString.ToUpper())
        {
            if (c == '\b') // Backspace
            {
                if (playerInput.Length > 0)
                    playerInput = playerInput.Substring(0, playerInput.Length - 1);
            }
            else if (c == '\n' || c == '\r') // Enter
            {
                CheckWord();
            }
            else
            {
                if (playerInput.Length < MAX_INPUT_LENGTH)
                {
                    playerInput += c;
                }
            }
        }

        inputDisplay.text = playerInput;
    }
    private void SnapPlayerToComputer()
    {
        player.transform.position = playerSpot.position;
        player.transform.rotation = playerSpot.rotation;

        if (player.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
        }
    }
    public void ShowStartScreen()
    {
        isOpened = true;

        SnapPlayerToComputer();
        blackScreen.SetActive(false);
        player.SetInput(false);

        hasStarted = false;
        isActive = false;
        hasWon = false;

        startPrompt.gameObject.SetActive(true);
        startPrompt.text = "Press SPACE to start";
        wordDisplay.text = "";
        inputDisplay.text = "";
        streakDisplay.text = $"0/{streakToWin}";
        timerBar.fillAmount = 1f;

        ui.gameObject.SetActive(true);
    }

    private void StartRealGame()
    {
        hasStarted = true;
        hasWon = false;
        wordsTyped = 0;
        correctInRow = 0;
        streakDisplay.text = $"0/{streakToWin}";
        player.SetInput(false);
        startPrompt.gameObject.SetActive(false);
        NextWord();
        isActive = true;
    }

    private void StopMinigame()
    {
        isActive = false;
        hasStarted = false;
        hasWon = false;
        isOpened = false;

        ui.SetActive(false);
        blackScreen.SetActive(true);
        player.SetInput(true);
    }

    private void EndSession()
    {
        isActive = false;
        wordDisplay.text = "SESSION OVER";
        inputDisplay.text = "";
        streakDisplay.text = $"0/{streakToWin}";
        Invoke(nameof(ShowStartScreen), 2f);
    }

    private void NextWord()
    {
        if (wordsTyped >= wordsPerSession)
        {
            EndSession();
            return;
        }

        playerInput = "";
        timeLeft = timeLimit;
        currentWord = wordPool[Random.Range(0, wordPool.Length)];
        wordDisplay.text = currentWord;
        inputDisplay.text = playerInput;
    }

    private void CheckWord()
    {
        wordsTyped++;
        string trimmedInput = playerInput.Trim().ToUpper();
        string trimmedWord = currentWord.Trim().ToUpper();

        if (trimmedInput.Equals(trimmedWord))
        {
            correctInRow++;
        }
        else
        {
            correctInRow = 0;
        }

        streakDisplay.text = $"{correctInRow}/{streakToWin}";

        if (correctInRow >= streakToWin)
        {
            WinGame();
            return;
        }

        if (wordsTyped >= wordsPerSession)
        {
            EndSession();
            return;
        }

        NextWord();
    }

    private void WinGame()
    {
        isActive = false;
        hasWon = true;
        wordDisplay.text = "YOU WON! \n [SPACE] to restart";
        streakDisplay.text = $"{streakToWin}/{streakToWin}";
        Completed?.Invoke();
    }
}
