using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class QuestionnaireData : MonoBehaviour
{
    public InputField Description;
    public ToggleGroup Confident;
    public ToggleGroup HowXWasThis;
    public ToggleGroup HowMuchLike;
    public ToggleGroup HowDifficult;
    public ToggleGroup HowEnjoyable;
    public GameObject ContinueButton;

    private bool showButton;

    public string DateText;
    public string DescrptionText;
    public string ConfidentText;
    public string HowXWasThisText;
    public string HowMuchLikeText;
    public string HowDifficultText;
    public string HowEnjoyableText;

    private const string TimeFormat = "dd/MM/yyyy-h:mm";

    void Update()
    {

        showButton = true;

        DateTime time = DateTime.Now;
        DateText = time.ToString(TimeFormat);

        DescrptionText = Description.text;
        if (DescrptionText == "")
            showButton = false;

        ConfidentText = Confident.GetActive() ? Confident.GetActive().ToString() : "";
        if (ConfidentText == "")
            showButton = false;


        HowXWasThisText = HowXWasThis.GetActive() ? HowXWasThis.GetActive().ToString() : "";
        if (HowXWasThisText == "")
            showButton = false;

        HowMuchLikeText = HowMuchLike.GetActive() ? HowMuchLike.GetActive().ToString() : "";
        if (HowMuchLikeText == "")
            showButton = false;

        HowDifficultText = HowDifficult.GetActive() ? HowDifficult.GetActive().ToString() : "";
        if (HowDifficultText == "")
            showButton = false;

        HowEnjoyableText = HowEnjoyable.GetActive() ? HowEnjoyable.GetActive().ToString() : "";
        if (HowEnjoyableText == "")
            showButton = false;

        ContinueButton.SetActive(showButton);


    }

    public string QuestionnaireDataToDatabaseFormat()
    {
        DateTime time = DateTime.Now;
        DateText = time.ToString(TimeFormat);



        

        DescrptionText = Description.text;
        ConfidentText = Confident.GetActive().ToString();
        HowXWasThisText = HowXWasThis.GetActive().ToString();
        HowMuchLikeText = HowMuchLike.GetActive().ToString();
        HowDifficultText = HowDifficult.GetActive().ToString();
        HowEnjoyableText = HowEnjoyable.GetActive().ToString();

        return
            string.Format(
                "&Date={0}&Description={1}&Confidence={2}&HowXIsThis={3}&HowMuchILikedThis={4}&HowDifficult={5}&HowEnjoyable={6}",
                DateText,
                DescrptionText,
                ConfidentText,
                HowXWasThisText,
                HowMuchLikeText,
                HowDifficultText,
                HowEnjoyableText);
    }

}

 public static class ToggleGroupExtension
 {
     public static Toggle GetActive(this ToggleGroup aGroup)
     {
         return aGroup.ActiveToggles().FirstOrDefault();
     }
 }