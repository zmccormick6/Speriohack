using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour 
{
	public string levelOne;
	public string levelTwo;
	public string levelThree;
    public string mainMenu;

	public void loadLevelOne()
	{
		SceneManager.LoadScene (levelOne);
	}

	public void loadLevelTwo()
	{
		SceneManager.LoadScene (levelTwo);
	}

	public void loadLevelThree()
	{
		SceneManager.LoadScene (levelThree);
	}

    public void loadMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
}
