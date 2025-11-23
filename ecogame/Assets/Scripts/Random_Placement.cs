using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class RandomPlacement : MonoBehaviour
{
    public RectTransform spawnArea;
    public List<RectTransform> buttons;    public int maxTries = 50;
    
    void Start()
    {
        PlaceButtonsRandomly();
    }

    void PlaceButtonsRandomly()
    {
        List<Rect> placedRects = new List<Rect>();

        foreach (RectTransform btn in buttons)
        {
            bool placed = false;
            int tries = 0;

            while (!placed && tries < maxTries)
            {
                tries++;
                Vector2 randomPos = new Vector2(
                    Random.Range(-(spawnArea.rect.width / 2 - btn.rect.width / 2),
                                 (spawnArea.rect.width / 2 - btn.rect.width / 2)),
                    Random.Range(-(spawnArea.rect.height / 2 - btn.rect.height / 2),
                                 (spawnArea.rect.height / 2 - btn.rect.height / 2))
                );

                btn.anchoredPosition = randomPos;

                Rect candidateRect = new Rect(
                    btn.anchoredPosition - btn.rect.size / 2,
                    btn.rect.size
                );

                if (!IsOverlapping(candidateRect, placedRects))
                {
                    placedRects.Add(candidateRect);
                    placed = true;
                }
            }

            if (!placed)
                Debug.LogWarning($"Impossible de placer {btn.name} sans chevauchement.");
        }
    }

    bool IsOverlapping(Rect rect, List<Rect> others)
    {
        foreach (Rect o in others)
        {
            if (rect.Overlaps(o))
                return true;
        }
        return false;
    }
}
