using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class QuestionnaireData2 : MonoBehaviour
{
    // question 1
    public InputField GroundDescription_Input;
    public InputField AirDescription_Input;
    public GameObject ContinueButton1;
    public Button Button1;

    public string GroundDescription;
    public string AirDescription;
    private bool ShowContinueButton1;

    // question 2
    public ToggleGroup Twitchy_Toggle;
    public ToggleGroup Fluid_Toggle;
    public ToggleGroup Stiff_Toggle;
    public ToggleGroup Floaty_Toggle;
    public ToggleGroup Responsive_Toggle;
    public GameObject ContinueButton2;

    public string Twitchy;
    public string Fluid;
    public string Stiff;
    public string Floaty;
    public string Responsive;
    private bool ShowContinueButton2;

    // question 3
    public ToggleGroup Enjoyable_Toggle;
    public ToggleGroup Difficult_Toggle;
    public ToggleGroup HowMuchLike_Toggle;
    public ToggleGroup HowFrustrated_Toggle;
    public GameObject ContinueButton3;

    public string Enjoyable;
    public string Difficult;
    public string HowMuchLike;
    public string HowFrustrated;
    private bool ShowContinueButton3;

    public string DateText;

    private const string TimeFormat = "dd/MM/yyyy-h:mm";

    void Start()
    {
        DateTime time = DateTime.Now;
        DateText = time.ToString(TimeFormat);
    }

    void FixedUpdate()
    {
        ShowContinueButton1 = true;
        ShowContinueButton2 = true;
        ShowContinueButton3 = true;


        // 1 - descriptions
        GroundDescription = GroundDescription_Input.text;
        AirDescription = AirDescription_Input.text;

        if (GroundDescription == "" || AirDescription == "")
            ShowContinueButton1 = false;

        // ------------

        // 2 - likert scale
        Twitchy = Twitchy_Toggle.GetActive() ? Twitchy_Toggle.GetActive().ToString() : "";
        Fluid = Fluid_Toggle.GetActive() ? Fluid_Toggle.GetActive().ToString() : "";
        Stiff = Stiff_Toggle.GetActive() ? Stiff_Toggle.GetActive().ToString() : "";
        Floaty = Floaty_Toggle.GetActive() ? Floaty_Toggle.GetActive().ToString() : "";
        Responsive = Responsive_Toggle.GetActive() ? Responsive_Toggle.GetActive().ToString() : "";

        if (Twitchy == "" ||
            Fluid == "" ||
            Stiff == "" ||
            Floaty == "" ||
            Responsive == "")
            ShowContinueButton2 = false;
        // ----------------

        // 3 - enjoyable etc.

        Enjoyable = Enjoyable_Toggle.GetActive() ? Enjoyable_Toggle.GetActive().ToString() : "";
        Difficult = Difficult_Toggle.GetActive() ? Difficult_Toggle.GetActive().ToString() : "";
        HowMuchLike = HowMuchLike_Toggle.GetActive() ? HowMuchLike_Toggle.GetActive().ToString() : "";
        HowFrustrated = HowFrustrated_Toggle.GetActive() ? HowFrustrated_Toggle.GetActive().ToString() : "";

        if (Enjoyable == "" ||
            Difficult == "" ||
            HowMuchLike == "" ||
            HowFrustrated == "")
            ShowContinueButton3 = false;

        // --------

        ContinueButton1.SetActive(ShowContinueButton1);
        ContinueButton2.SetActive(ShowContinueButton2);
        ContinueButton3.SetActive(ShowContinueButton3);


    }

    public string QuestionnaireDataToDatabaseFormat()
    {
        DateTime time = DateTime.Now;
        DateText = time.ToString(TimeFormat);

            // GroundDescription, AirDescription
            // Twitchy, Fluid, Stiff, Floaty, Responsive
            // Enjoyable, Difficult, HowMuchLike, HowFrustrated


        return
            string.Format(
                "&Date={0}&GroundDescription={1}&AirDescription={2}&Twitchy={3}&Fluid={4}&Stiff={5}&Floaty={6}&Responsive={7}&Enjoyable={8}&Difficult={9}&HowMuchLike={10}&HowFrustrated={11}",
                DateText,
                GroundDescription,
                AirDescription,
                Twitchy,
                Fluid,
                Stiff,
                Floaty,
                Responsive,
                Enjoyable,
                Difficult,
                HowMuchLike,
                HowFrustrated);
    }

}