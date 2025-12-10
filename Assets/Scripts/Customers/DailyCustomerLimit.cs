using UnityEngine;

public class DailyCustomerLimit : MonoBehaviour {
    public static DailyCustomerLimit Instance;

    public int dailyLimit = 20;
    private int servedCount = 0;

    public bool DayIsOver => servedCount >= dailyLimit;

    void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterServedCustomer() {
        servedCount++;

        if (servedCount >= dailyLimit) {
            Debug.Log("Daily limit reached!");
        }
    }

    public int GetServedCount() => servedCount;

    public void ResetDay() {
        servedCount = 0;
    }
}