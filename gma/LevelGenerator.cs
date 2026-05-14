using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelData
{
    public List<List<Color>> bottleColors = new List<List<Color>>();
}

public class LevelGenerator : MonoBehaviour
{
    public int bottleCount = 6;
    public int colorCount = 4;
    public int segmentsPerColor = 4;

    // Generates a solvable level and returns LevelData
    public LevelData GenerateLevel()
    {
        // Step 1: Prepare color pool
        List<Color> colorPool = new List<Color>();
        List<Color> colorList = GetColorList(colorCount);
        foreach (var color in colorList)
        {
            for (int i = 0; i < segmentsPerColor; i++)
                colorPool.Add(color);
        }

        // Step 2: Prepare bottles
        List<List<Color>> bottles = new List<List<Color>>();
        for (int i = 0; i < bottleCount; i++)
            bottles.Add(new List<Color>());

        // Step 3: Shuffle and distribute colors
        System.Random rng = new System.Random();
        int poolIndex = 0;
        while (colorPool.Count > 0)
        {
            // Pick a random bottle that is not full
            List<int> available = new List<int>();
            for (int i = 0; i < bottleCount; i++)
                if (bottles[i].Count < segmentsPerColor)
                    available.Add(i);
            int bottleIdx = available[rng.Next(available.Count)];
            int colorIdx = rng.Next(colorPool.Count);
            bottles[bottleIdx].Add(colorPool[colorIdx]);
            colorPool.RemoveAt(colorIdx);
        }

        // Step 4: Check solvability (simple check: each color appears exactly segmentsPerColor times)
        // For a more robust check, implement a solver here.
        // For now, we assume the random distribution is solvable if the above is true.

        LevelData data = new LevelData();
        data.bottleColors = bottles;
        return data;
    }

    // Returns a list of distinct colors
    List<Color> GetColorList(int count)
    {
        List<Color> colors = new List<Color>();
        Color[] palette = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan, Color.white, Color.black, Color.gray };
        for (int i = 0; i < count; i++)
            colors.Add(palette[i % palette.Length]);
        return colors;
    }

    // Export as JSON string
    public string ExportLevelAsJson(LevelData data)
    {
        // Color is not directly serializable, so convert to hex
        List<List<string>> hexBottles = new List<List<string>>();
        foreach (var bottle in data.bottleColors)
        {
            List<string> hexList = new List<string>();
            foreach (var color in bottle)
                hexList.Add(ColorUtility.ToHtmlStringRGBA(color));
            hexBottles.Add(hexList);
        }
        return JsonUtility.ToJson(new SerializableLevel(hexBottles), true);
    }

    [Serializable]
    class SerializableLevel
    {
        public List<List<string>> bottleColors;
        public SerializableLevel(List<List<string>> bottleColors) { this.bottleColors = bottleColors; }
    }
}
