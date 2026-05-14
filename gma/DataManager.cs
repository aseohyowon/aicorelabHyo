using UnityEngine;

public class DataManager : MonoBehaviour
{
    private const string LevelKey = "CurrentLevel";
    private const string CoinsKey = "TotalCoins";

    public static DataManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(LevelKey, 1);
    }

    public void UnlockNextLevel()
    {
        int nextLevel = GetCurrentLevel() + 1;
        PlayerPrefs.SetInt(LevelKey, nextLevel);
        PlayerPrefs.Save();
    }

    public int GetTotalCoins()
    {
        return PlayerPrefs.GetInt(CoinsKey, 0);
    }

    public void AddCoins(int amount)
    {
        int coins = GetTotalCoins() + amount;
        PlayerPrefs.SetInt(CoinsKey, coins);
        PlayerPrefs.Save();
    }
}
