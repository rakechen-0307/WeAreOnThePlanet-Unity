using TMPro;
using UnityEngine;

public class AchievementBar : MonoBehaviour
{
    public TMP_Text achievementName;

    [SerializeField]
    private CustomUI progressBar;

    public void setProgress(float progress, float maxCapacity)
    {
        float percentage = Mathf.Clamp01(progress / maxCapacity);
        progressBar.setSize(new Vector2(percentage, 1f));
        progressBar.Resize();
    }
}
