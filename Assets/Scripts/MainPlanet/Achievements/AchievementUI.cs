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

    [SerializeField]
    private GameObject cursorUI;

    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private MessengerManager messengerManager;

    private void Start()
    {
        achievementUI.SetActive(false);
    }

    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (achievementUI.activeInHierarchy)
            {
                CloseMenu();
            }
            else
            {
                if (messengerManager._messengerIsOpened)
                {
                    bool close = await messengerManager.CloseMessenger();
                }
                OpenMenu();
            }
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

    private void OpenMenu()
    {
        achievementUI.SetActive(true);
        playerMovement.moveable = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        showAchievements();
    }
    public void CloseMenu()
    {
        achievementUI.SetActive(false);
        playerMovement.moveable = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
