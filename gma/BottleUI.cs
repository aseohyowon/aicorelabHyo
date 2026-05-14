using System.Collections.Generic;
using UnityEngine;

public class BottleUI : MonoBehaviour
{
    public List<SpriteRenderer> segments = new List<SpriteRenderer>(4);

    // Call this to update the bottle visuals
    public void UpdateVisuals(List<Color> colors)
    {
        for (int i = 0; i < segments.Count; i++)
        {
            if (i < colors.Count)
            {
                segments[i].color = colors[i];
                segments[i].enabled = true;
            }
            else
            {
                segments[i].enabled = false;
            }
        }
    }

    // Optionally, auto-assign SpriteRenderers if not set in inspector
    void Reset()
    {
        segments.Clear();
        foreach (Transform child in transform)
        {
            var sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
                segments.Add(sr);
        }
    }
}
