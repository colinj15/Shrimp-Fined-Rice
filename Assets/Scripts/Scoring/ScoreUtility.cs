using UnityEngine;

// converts minigame raw scores into percents/weighted score out of 20
// for customer scoring purposes
public static class ScoreUtility {
    public static int ToPercent(int score, int maxScore) {
        if (maxScore <= 0) return 0;

        float normalized = Mathf.Clamp01((float)score / maxScore);
        return Mathf.RoundToInt(normalized * 100f);
    }

    public static int ToWeighted20(int score, int maxScore) {
        if (maxScore <= 0) return 0;

        float normalized = Mathf.Clamp01((float)score / maxScore);
        return Mathf.RoundToInt(normalized * 20f);
    }
}
