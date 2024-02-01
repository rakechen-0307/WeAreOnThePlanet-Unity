using UnityEngine;

public class AchievementUI : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;

    [SerializeField]
    private GameObject achievementBarPrefab;

    [SerializeField]
    private Transform achievementContent;

    [SerializeField]
    private GameObject achievementUI;

    private void Start()
    {
        achievementUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            achievementUI.SetActive(!achievementUI.activeInHierarchy);
            showAchievements();
        }
    }

    private void showAchievements()
    {
        foreach (Transform child in achievementContent)
        {
            Destroy(child.gameObject);
        }
        foreach (Achievement achievement in loadedData.achievements)
        {
            AchievementBar newAchievementBar = Instantiate(achievementBarPrefab, achievementContent).GetComponent<AchievementBar>();
            newAchievementBar.achievementName.text = achievement.name;
            newAchievementBar.setProgress(achievement.progress, achievement.maxProgress);
        }
    }
}
