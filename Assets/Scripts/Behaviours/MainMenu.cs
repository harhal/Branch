using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Button NewGameButton;
    public Button ContinueButton;
    public Text PofileNameText;

    public string ProfileName = "TestPlayer";

	// Use this for initialization
	void Awake ()
    {
        PofileNameText.text = ProfileName;
        CheckExistingProfile();
    }

    public void ChangeProfileName(string newName)
    {
        ProfileName = newName;
        PofileNameText.text = ProfileName;
        CheckExistingProfile();
    }

    public bool CheckExistingProfile()
    {
        bool IsExistingProfile = (File.Exists("Saves/" + ProfileName + "/OnlySave.json"));
        NewGameButton.gameObject.SetActive(!IsExistingProfile);
        ContinueButton.gameObject.SetActive(IsExistingProfile);
        return IsExistingProfile;
    }

    public void NewGame()
    {
        if (!Directory.Exists("Saves/" + ProfileName))
            Directory.CreateDirectory("Saves/" + ProfileName);
        File.Copy("Assets/Data/InitSessionData.json", "Saves/" + ProfileName + "/OnlySave.json");
        ContinueGame();
    }

    public void ContinueGame()
    {
        SessionData.ProfileName = ProfileName;
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.CancelQuit();
    }
}
