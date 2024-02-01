using UnityEngine;

public class AchievementUI : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;

    [SerializeField]
    private GameObject achievementBarPrefab;

    [SerializeField]
    private Transform achievementContent;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
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
