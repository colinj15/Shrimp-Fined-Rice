using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class FryingGameManager : MonoBehaviour {
    
    public RectTransform targetRing;       // outer ring
    public RectTransform innerCircle;      // inner circle that grows
    public TextMeshProUGUI scoreText;

    // sprites
    public Transform panSprite;
    public Transform foodSprite;

    // "settings"
    public int totalRounds = 5;
    public float minGrowthSpeed = 2f;
    public float maxGrowthSpeed = 5f;
    public float minDelayBetweenRounds = 0f;
    public float maxDelayBetweenRounds = 2f;

    public Button nextGame; 

    private bool gameOver = false; 

    private float currentSpeed;
    private bool isGrowing = false;
    private int currentRound = 0;
    private float currentScale = 0f;
    private Vector3 originalClickPos;
    private bool roundActive = false;
    private int totalScore = 0;
    private Vector3 originalPanPos;
    private Vector3 originalFoodPos;


    void Start(){
        foodSprite.gameObject.SetActive(true);
        panSprite.gameObject.SetActive(true);

        originalPanPos = panSprite.position;
        originalFoodPos = foodSprite.position;

        scoreText.text = "";
        StartCoroutine(GameLoop());

        nextGame.gameObject.SetActive(false); //hides return to kitchen button initially
    }

    void Update() {
        if (isGrowing) {
            currentScale += currentSpeed * Time.deltaTime;
            innerCircle.localScale = Vector3.one * currentScale;

            if (currentScale > targetRing.localScale.x * 1.2f) { // failsafe if player never clicks 
                EndRound(0);
            }
        }

        if (roundActive && Input.GetMouseButtonDown(0)) {
            // 10 units up, 1.2 scale
            StartCoroutine(JumpSprite(panSprite, new Vector3(0, 1f, 0), 1.2f, 0.5f));
            // 50 units up, 1.35 scale
            StartCoroutine(JumpSprite(foodSprite, new Vector3(0, 3f, 0), 1.35f, 0.5f));
            EvaluateClick();
        }
    }

    IEnumerator GameLoop() {
        while (currentRound < totalRounds) {
            yield return new WaitForSeconds(Random.Range(minDelayBetweenRounds, maxDelayBetweenRounds));
            StartRound();
            yield return new WaitUntil(() => roundActive == false);
        }

        if (currentRound >= 5) {
            EndGame();
            targetRing.gameObject.SetActive(false);
            innerCircle.gameObject.SetActive(false);

            //yield return scoreText.text;
        }
    }

    void StartRound() {
        currentRound++;
        currentScale = 0f;
        currentSpeed = Random.Range(minGrowthSpeed, maxGrowthSpeed);
        innerCircle.localScale = Vector3.zero;
        isGrowing = true;
        roundActive = true;
    }

    void EvaluateClick() {
        if (!roundActive) return;

        isGrowing = false;
        roundActive = false;

        float targetSize = targetRing.localScale.x;
        float circleSize = innerCircle.localScale.x;
        float ratio = circleSize / targetSize;

        int roundScore = 0;

        if (ratio >= 0.95f && ratio <= 1.05f)
            roundScore = 100;
        else if (ratio >= 0.80f && ratio <= 1.20f)
            roundScore = 75;
        else if (ratio >= 0.70f && ratio <= 1.30f)
            roundScore = 50;
        else
            roundScore = 0;

        EndRound(roundScore);
    }


    void EndRound(int roundScore) {
        isGrowing = false;
        roundActive = false;

        totalScore += roundScore;

        scoreText.text = $"{roundScore} pts\nTotal: {totalScore} pts";
    }

    void EndGame() {
        gameOver = true;
        nextGame.gameObject.SetActive(true);
        Debug.Log ("Game over; total score: " + totalScore);

        //save score to order
        var order = OrderSystem.ActiveMinigameOrder;
        if (order != null) {
            int weighted = GetWeightedScore();
            OrderManager.Instance.AddScore(order.CustomerID, weighted, OrderManager.MinigameType.Frying);
            OrderManager.Instance.MarkMinigameComplete(order.CustomerID, OrderManager.MinigameType.Frying);
        }
    }

    public void OnNextGameButtonPressed() {
        if (gameOver) {
            // SceneManager.LoadScene("NAME OF NEXT SCENE HERE"); 
        }
    }

    IEnumerator JumpSprite(Transform sprite, Vector3 jumpOffset, float yScaleMultiplier, float duration) {
        Vector3 startPos = sprite.position;
        Vector3 endPos = startPos + jumpOffset;

        Vector3 startScale = sprite.localScale;
        Vector3 endScale = new Vector3(startScale.x, startScale.y * yScaleMultiplier, startScale.z);

        float elapsed = 0f;

        // Move up and stretch
        while (elapsed < duration / 2f) {
            float t = elapsed / (duration / 2f);
            sprite.position = Vector3.Lerp(startPos, endPos, t);
            sprite.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Move back down and shrink
        elapsed = 0f;
        while (elapsed < duration / 2f) {
            float t = elapsed / (duration / 2f);
            sprite.position = Vector3.Lerp(endPos, startPos, t);
            sprite.localScale = Vector3.Lerp(endScale, startScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset just in case
        sprite.position = startPos;
        sprite.localScale = startScale;

    }

    public int GetWeightedScore() {
        int maxScore = totalRounds * 100;
        return ScoreUtility.ToWeighted20(totalScore, maxScore);
    }

}
