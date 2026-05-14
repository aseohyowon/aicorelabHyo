        // Coroutine for a simple bounce animation
        public System.Collections.IEnumerator Bounce(float bounceHeight = 0.2f, float duration = 0.2f)
        {
            Vector3 startPos = transform.position;
            Vector3 upPos = startPos + Vector3.up * bounceHeight;
            float t = 0f;
            // Move up
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                transform.position = Vector3.Lerp(startPos, upPos, t);
                yield return null;
            }
            // Move down
            t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                transform.position = Vector3.Lerp(upPos, startPos, t);
                yield return null;
            }
            transform.position = startPos;
        }
    // Coroutine to tilt and move bottle for pouring
    public System.Collections.IEnumerator AnimatePourTo(WaterSortBottle targetBottle, float duration = 0.5f, float tiltAngle = 45f, float moveDistance = 0.5f)
    {
        // Save original position and rotation
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        // Calculate target position (move towards target bottle)
        Vector3 dir = (targetBottle.transform.position - transform.position).normalized;
        Vector3 pourPos = startPos + dir * moveDistance;

        // Calculate target rotation (tilt towards target)
        float angle = tiltAngle * Mathf.Sign(dir.x);
        Quaternion pourRot = Quaternion.Euler(0, 0, angle);

        // Animate to pour position/rotation
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(startPos, pourPos, t);
            transform.rotation = Quaternion.Lerp(startRot, pourRot, t);
            yield return null;
        }
        transform.position = pourPos;
        transform.rotation = pourRot;

        // Wait for pouring (customize duration as needed)
        yield return new WaitForSeconds(0.5f);

        // Animate back to original position/rotation
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(pourPos, startPos, t);
            transform.rotation = Quaternion.Lerp(pourRot, startRot, t);
            yield return null;
        }
        transform.position = startPos;
        transform.rotation = startRot;
    }
using System.Collections.Generic;
using UnityEngine;

public class WaterSortBottle : MonoBehaviour
{
    // List to store colors in the bottle (bottom to top)
    public List<Color> Colors { get; private set; } = new List<Color>();

    // Maximum number of colors the bottle can hold
    public const int MaxCapacity = 4;

    // Property to check if the bottle is full
    public bool IsFull => Colors.Count >= MaxCapacity;

    // Property to check if the bottle is empty
    public bool IsEmpty => Colors.Count == 0;


    // Add a color to the bottle (if not full)
    public bool AddColor(Color color)
    {
        if (IsFull)
            return false;
        Colors.Add(color);
        return true;
    }

    // Check if pouring to targetBottle is allowed
    public bool CanPour(WaterSortBottle targetBottle)
    {
        if (this.IsEmpty) return false;
        if (targetBottle.IsFull) return false;
        if (targetBottle.IsEmpty) return true;
        // Check if top colors match
        Color myTop = this.Colors[this.Colors.Count - 1];
        Color targetTop = targetBottle.Colors[targetBottle.Colors.Count - 1];
        return myTop.Equals(targetTop);
    }

    // Pour all matching top colors to targetBottle
    public bool PourTo(WaterSortBottle targetBottle)
    {
        if (!CanPour(targetBottle)) return false;

        Color topColor = Colors[Colors.Count - 1];
        int moveCount = 0;
        // Count how many top colors match
        for (int i = Colors.Count - 1; i >= 0; i--)
        {
            if (Colors[i].Equals(topColor))
                moveCount++;
            else
                break;
        }

        // Check target capacity
        int space = WaterSortBottle.MaxCapacity - targetBottle.Colors.Count;
        moveCount = Mathf.Min(moveCount, space);

        for (int i = 0; i < moveCount; i++)
        {
            Color c = Colors[Colors.Count - 1];
            Colors.RemoveAt(Colors.Count - 1);
            targetBottle.Colors.Add(c);
        }
        return moveCount > 0;
    }

    // Remove the top color from the bottle (if not empty)
    public bool RemoveTopColor(out Color color)
    {
        if (IsEmpty)
        {
            color = default;
            return false;
        }
        int lastIndex = Colors.Count - 1;
        color = Colors[lastIndex];
        Colors.RemoveAt(lastIndex);
        return true;
    }
}
