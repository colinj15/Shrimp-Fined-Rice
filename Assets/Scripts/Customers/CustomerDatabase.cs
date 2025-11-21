using System.Collections.Generic;
using UnityEngine;

public class CustomerDatabase : MonoBehaviour {
    public static CustomerDatabase Instance;

    [System.Serializable]
    public class CharacterData {
        public int CustomerID;
        public string CustomerName;
        public Sprite LobbySprite;
        public Sprite WaitingSprite;
    }

    public List<CharacterData> characters = new List<CharacterData>();
    private List<int> usedIDs = new List<int>();

    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // get a random character not used yet
    public CharacterData GetUniqueRandomCharacter() {
        if (usedIDs.Count == characters.Count)
            usedIDs.Clear();  // allow repeats only when needed

        CharacterData selected;
        do {
            selected = characters[Random.Range(0, characters.Count)];
        }
        while (usedIDs.Contains(selected.CustomerID));

        usedIDs.Add(selected.CustomerID);
        return selected;
    }

    // placeholder for now
    public List<string> GenerateRandomIngredients() {
        List<string> sample = new List<string>() { "Bok choy", "Broccoli", "Carrots"};

        int count = Random.Range(1, 4);

        List<string> ingredients = new List<string>();
        for (int i = 0; i < count; i++) {
            string ingredient = sample[Random.Range(0, sample.Count)];
            if (!ingredients.Contains(ingredient))
                ingredients.Add(ingredient);
        }

        return ingredients;
    }
}

