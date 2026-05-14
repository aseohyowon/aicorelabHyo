using System;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    // Singleton pattern for easy access
    public static AdManager Instance { get; private set; }

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

    // Placeholder for showing a rewarded ad
    public void ShowRewardedAd(Action onComplete)
    {
        Debug.Log("Showing rewarded ad (placeholder)...");
        // Simulate ad watching delay
        // In real implementation, call onComplete after ad is finished
        onComplete?.Invoke();
    }
}
