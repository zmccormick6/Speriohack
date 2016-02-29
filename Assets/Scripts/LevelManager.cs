using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Button LevelTwoButton;
    public Button LevelThreeButton;

    public void Awake()
    {
        UpdateButtons();
    }

	public void resetLevels()
	{
		PlayerPrefs.SetInt("level_two_enabled", 0);
		PlayerPrefs.SetInt("level_three_enabled", 0);

        UpdateButtons();
	}

    private void UpdateButtons()
    {
        var levelTwoEnabeled = PlayerPrefs.GetInt("level_two_enabled", 0);
        var levelThreeEnabeled = PlayerPrefs.GetInt("level_three_enabled", 0);

        LevelTwoButton.interactable = levelTwoEnabeled == 1;
        LevelThreeButton.interactable = levelThreeEnabeled == 1;
    }
}