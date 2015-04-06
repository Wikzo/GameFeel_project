using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class DemographicsInput : MonoBehaviour
{
    public InputField Name;
    public ToggleGroup Gender;
    public InputField Age;
    public ToggleGroup Region;
    public ToggleGroup ExperienceGames;
    public ToggleGroup ExperiencePlatformers;

    public GameObject ContinueButton;
    public GameObject Agreement;
    private bool showButton;

    public string NameText;
    public string GenderText;
    public string AgeText;
    public string RegionText;
    public string ExperienceGamesText;
    public string ExperiencePlatformersText;

    void Update()
    {

        showButton = true;

        NameText = Name.text;
        if (NameText == "")
            showButton = false;

        GenderText = Gender.GetActive() ? Gender.GetActive().ToString() : "";
        if (GenderText == "")
            showButton = false;

        AgeText = Age.text;
        if (AgeText == "")
            showButton = false;

        RegionText = Region.GetActive() ? Region.GetActive().ToString() : "";
        if (RegionText == "")
            showButton = false;

        ExperienceGamesText = ExperienceGames.GetActive() ? ExperienceGames.GetActive().ToString() : "";
        if (ExperienceGamesText == "")
            showButton = false;

        ExperiencePlatformersText = ExperiencePlatformers.GetActive() ? ExperiencePlatformers.GetActive().ToString() : "";
        if (ExperiencePlatformersText == "")
            showButton = false;

        ContinueButton.SetActive(showButton);
        Agreement.SetActive(showButton);



        Demographics.Instance.YourName = NameText;
        Demographics.Instance.Gender = GenderText;
        Demographics.Instance.Age = AgeText;
        Demographics.Instance.Country = RegionText;
        Demographics.Instance.ExperienceGames = ExperienceGamesText;
        Demographics.Instance.ExperiencePlatformers = ExperiencePlatformersText;
    }

}
