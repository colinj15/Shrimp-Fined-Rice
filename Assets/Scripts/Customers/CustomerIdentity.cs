using UnityEngine;

public class CustomerIdentity : MonoBehaviour {
    public int CustomerID;
    public string CustomerName;

    public Sprite LobbySprite;     // side-facing
    public Sprite WaitingSprite;   // front-facing

    private SpriteRenderer sr;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    // Called by queue manager or waiting manager
    public void Initialize(CustomerDatabase.CharacterData data) {
        CustomerID = data.CustomerID;
        CustomerName = data.CustomerName;

        LobbySprite = data.LobbySprite;
        WaitingSprite = data.WaitingSprite;

        // Default for lobby
        SetLobbySprite();
    }

    public void SetLobbySprite() {
        sr.sprite = LobbySprite;
    }

    public void SetWaitingSprite() {
        sr.sprite = WaitingSprite;
    }
}

