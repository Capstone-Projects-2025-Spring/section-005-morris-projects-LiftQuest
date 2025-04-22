using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Round System")]
    public GameObject[] monsterRounds; // One prefab per round
    public Transform spawnPoint;
    public float spawnDelay = 1f;

    private int currentRound = 0;
    private bool roundInProgress = false;

    [SerializeField] private Text roundText;
    public CanvasGroup roundTextGroup;
    public float fadeDuration = 0.5f;
    public float holdDuration = 1.5f;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject loseScreen;

    [SerializeField] private Text scoret;
    public int score;


    private void Start()
    {
        StartCoroutine(StartNextRound());
    }

    IEnumerator StartNextRound()
    {
        roundInProgress = true;
        Debug.Log($"Starting Round {currentRound + 1}");
        string roundLabel = (currentRound == monsterRounds.Length - 1) ? "FINAL ROUND: BOSS" : $"Round {currentRound + 1}";
        yield return StartCoroutine(FadeRoundText(roundLabel));

        yield return new WaitForSeconds(spawnDelay);

        if (currentRound < monsterRounds.Length)
        {
            GameObject prefab = monsterRounds[currentRound];
            GameObject newMonster = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

            Monster monsterController = newMonster.GetComponent<Monster>();
            if (monsterController != null)
            {
                monsterController.spawner = this;

                // Make last round the boss
                if (currentRound == monsterRounds.Length - 1)
                {
                    monsterController.isBoss = true;
                }
            }

            UpdateRoundUI();
        }
        else
        {
            Debug.Log("All rounds completed! You win!");
        }
    }

    IEnumerator FadeRoundText(string message)
    {
        if (roundText == null || roundTextGroup == null)
            yield break;

        roundText.text = message;

        // Fade in
        yield return FadeCanvasGroup(roundTextGroup, 0f, 1f, fadeDuration);

        // Hold
        yield return new WaitForSeconds(holdDuration);

        // Fade out
        yield return FadeCanvasGroup(roundTextGroup, 1f, 0f, fadeDuration);
    }

    IEnumerator FadeCanvasGroup(CanvasGroup group, float startAlpha, float endAlpha, float duration)
    {
        float timer = 0f;
        group.alpha = startAlpha;
        while (timer < duration)
        {
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        group.alpha = endAlpha;
    }

    public void Win(){
        scoreText.text = "Score: " + score;
        winScreen.SetActive(true);
    }

    public void Lose(){
        loseScreen.SetActive(true);
    }
    public void OnMonsterDefeated()
    {
        currentRound++;
        if (currentRound < monsterRounds.Length)
        {
            StartCoroutine(StartNextRound());
        }else{
            Win();
        }
    }

    void UpdateRoundUI()
    {
        if (roundText != null)
        {
            roundText.text = $"Round {currentRound + 1}";
        }
    }

    public void UpdateScoreUI()
    {
        if (scoret != null)
        {
            scoret.text = $"Score: {score.ToString("D3")}";
        }
    }
}
