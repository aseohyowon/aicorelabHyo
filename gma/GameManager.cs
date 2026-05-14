using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<WaterSortBottle> bottles = new List<WaterSortBottle>();

    // Assign this in the inspector with your WaterSortBottle prefab
    public GameObject bottlePrefab;

    private WaterSortBottle selectedBottle = null;

    // Stack for undo system
    private Stack<List<List<Color>>> undoStack = new Stack<List<List<Color>>>();

    // Free use counters
    private int freeUndoCount = 3;
    private int freeAddBottleCount = 3;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                WaterSortBottle bottle = hit.collider.GetComponent<WaterSortBottle>();
                if (bottle != null)
                {
                    OnBottleClicked(bottle);
                }
            }
        }
    }

    void OnBottleClicked(WaterSortBottle bottle)
    {
        if (selectedBottle == null)
        {
            selectedBottle = bottle;
        }
        else if (selectedBottle != bottle)
        {
            if (selectedBottle.CanPour(bottle))
            {
                SaveStateForUndo();
                selectedBottle.PourTo(bottle);
                // Start bounce animation on target bottle
                StartCoroutine(bottle.Bounce());
                // Check win condition and play confetti if won
                if (CheckWinCondition() && VFXManager.Instance != null)
                {
                    VFXManager.Instance.PlayConfetti();
                }
            }
            selectedBottle = null;
        }
        else
        {
            selectedBottle = null;
        }
    }

    // Save current state for undo
    void SaveStateForUndo()
    {
        List<List<Color>> state = new List<List<Color>>();
        foreach (var bottle in bottles)
        {
            state.Add(new List<Color>(bottle.Colors));
        }
        undoStack.Push(state);
    }

    // Undo last move
    public void UndoMove()
    {
        if (undoStack.Count == 0) return;
        if (freeUndoCount > 0)
        {
            freeUndoCount--;
            PerformUndo();
        }
        else
        {
            // Require ad after 3 free uses
            if (AdManager.Instance != null)
            {
                AdManager.Instance.ShowRewardedAd(() => PerformUndo());
            }
            else
            {
                Debug.LogWarning("AdManager not found. Undo not available.");
            }
        }
    }

    private void PerformUndo()
    {
        var prevState = undoStack.Pop();
        for (int i = 0; i < bottles.Count && i < prevState.Count; i++)
        {
            bottles[i].Colors.Clear();
            bottles[i].Colors.AddRange(prevState[i]);
            // Optionally, update visuals if needed
            var ui = bottles[i].GetComponent<BottleUI>();
            if (ui != null)
                ui.UpdateVisuals(bottles[i].Colors);
        }
    }

    // Add an extra empty bottle to the scene
    public void AddExtraBottle()
    {
        if (bottlePrefab == null)
        {
            Debug.LogError("Bottle prefab not assigned!");
            return;
        }
        if (freeAddBottleCount > 0)
        {
            freeAddBottleCount--;
            PerformAddExtraBottle();
        }
        else
        {
            // Require ad after 3 free uses
            if (AdManager.Instance != null)
            {
                AdManager.Instance.ShowRewardedAd(() => PerformAddExtraBottle());
            }
            else
            {
                Debug.LogWarning("AdManager not found. AddExtraBottle not available.");
            }
        }
    }

    private void PerformAddExtraBottle()
    {
        Vector3 pos = new Vector3(bottles.Count * 2.0f, 0, 0); // Example layout
        GameObject newObj = Instantiate(bottlePrefab, pos, Quaternion.identity);
        WaterSortBottle newBottle = newObj.GetComponent<WaterSortBottle>();
        if (newBottle != null)
        {
            bottles.Add(newBottle);
        }
    }

    public bool CheckWinCondition()
    {
        foreach (var bottle in bottles)
        {
            if (bottle.IsEmpty)
                continue;
            if (bottle.Colors.Count != WaterSortBottle.MaxCapacity)
                return false;
            Color firstColor = bottle.Colors[0];
            for (int i = 1; i < bottle.Colors.Count; i++)
            {
                if (!bottle.Colors[i].Equals(firstColor))
                    return false;
            }
        }
        return true;
    }
}
