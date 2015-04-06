using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class QuestionnaireData : MonoBehaviour
{
    public Animator Animator;

    public InputField g_Description;
    public ToggleGroup g_Confident;
    public ToggleGroup g_HowXWasThis;
    public ToggleGroup g_HowMuchLike;
    public ToggleGroup g_HowDifficult;
    public ToggleGroup g_HowEnjoyable;
    public GameObject g_ContinueButton;

    public InputField a_Description;
    public ToggleGroup a_Confident;
    public ToggleGroup a_HowXWasThis;
    public ToggleGroup a_HowMuchLike;
    public ToggleGroup a_HowDifficult;
    public ToggleGroup a_HowEnjoyable;
    public GameObject a_ContinueButton;

    private bool g_showButton;
    private bool a_showButton;

    public string DateText;
    public string g_DescrptionText;
    public string g_ConfidentText;
    public string g_HowXWasThisText;
    public string g_HowMuchLikeText;
    public string g_HowDifficultText;
    public string g_HowEnjoyableText;

    public string a_DescrptionText;
    public string a_ConfidentText;
    public string a_HowXWasThisText;
    public string a_HowMuchLikeText;
    public string a_HowDifficultText;
    public string a_HowEnjoyableText;

    private const string TimeFormat = "dd/MM/yyyy-h:mm";

    void Start()
    {
        if (Animator == null)
            Debug.Log("Error - questionnare canvas needs animator component!");
    }

    void FixedUpdate()
    {

        g_showButton = true;
        a_showButton = true;


        // ground
        g_DescrptionText = g_Description.text;
        if (g_DescrptionText == "")
            g_showButton = false;

        g_ConfidentText = g_Confident.GetActive() ? g_Confident.GetActive().ToString() : "";
        if (g_ConfidentText == "")
            g_showButton = false;

        g_HowXWasThisText = g_HowXWasThis.GetActive() ? g_HowXWasThis.GetActive().ToString() : "";
        if (g_HowXWasThisText == "")
            g_showButton = false;

        g_HowMuchLikeText = g_HowMuchLike.GetActive() ? g_HowMuchLike.GetActive().ToString() : "";
        if (g_HowMuchLikeText == "")
            g_showButton = false;

        g_HowDifficultText = g_HowDifficult.GetActive() ? g_HowDifficult.GetActive().ToString() : "";
        if (g_HowDifficultText == "")
            g_showButton = false;

        g_HowEnjoyableText = g_HowEnjoyable.GetActive() ? g_HowEnjoyable.GetActive().ToString() : "";
        if (g_HowEnjoyableText == "")
            g_showButton = false;

        g_ContinueButton.SetActive(g_showButton);

        // air
        a_DescrptionText = a_Description.text;
        if (a_DescrptionText == "")
            a_showButton = false;

        a_ConfidentText = a_Confident.GetActive() ? a_Confident.GetActive().ToString() : "";
        if (a_ConfidentText == "")
            a_showButton = false;

        a_HowXWasThisText = a_HowXWasThis.GetActive() ? a_HowXWasThis.GetActive().ToString() : "";
        if (a_HowXWasThisText == "")
            a_showButton = false;

        a_HowMuchLikeText = a_HowMuchLike.GetActive() ? a_HowMuchLike.GetActive().ToString() : "";
        if (a_HowMuchLikeText == "")
            a_showButton = false;

        a_HowDifficultText = a_HowDifficult.GetActive() ? a_HowDifficult.GetActive().ToString() : "";
        if (a_HowDifficultText == "")
            a_showButton = false;

        a_HowEnjoyableText = a_HowEnjoyable.GetActive() ? a_HowEnjoyable.GetActive().ToString() : "";
        if (a_HowEnjoyableText == "")
            a_showButton = false;

        a_ContinueButton.SetActive(a_showButton);


    }

    public string QuestionnaireDataToDatabaseFormat()
    {
        DateTime time = DateTime.Now;
        DateText = time.ToString(TimeFormat);

        // ground
        g_DescrptionText = g_Description.text;
        g_ConfidentText = g_Confident.GetActive().ToString();
        g_HowXWasThisText = g_HowXWasThis.GetActive().ToString();
        g_HowMuchLikeText = g_HowMuchLike.GetActive().ToString();
        g_HowDifficultText = g_HowDifficult.GetActive().ToString();
        g_HowEnjoyableText = g_HowEnjoyable.GetActive().ToString();

        // air
        a_DescrptionText = a_Description.text;
        a_ConfidentText = a_Confident.GetActive().ToString();
        a_HowXWasThisText = a_HowXWasThis.GetActive().ToString();
        a_HowMuchLikeText = a_HowMuchLike.GetActive().ToString();
        a_HowDifficultText = a_HowDifficult.GetActive().ToString();
        a_HowEnjoyableText = a_HowEnjoyable.GetActive().ToString();

        return
            string.Format(
                "&Date={0}&G_Description={1}&G_Confidence={2}&G_HowXIsThis={3}&G_HowMuchILikedThis={4}&G_HowDifficult={5}&G_HowEnjoyable={6}&A_Description={7}&A_Confidence={8}&A_HowXIsThis={9}&A_HowMuchILikedThis={10}&A_HowDifficult={11}&A_HowEnjoyable={12}",
                DateText,
                g_DescrptionText,
                g_ConfidentText,
                g_HowXWasThisText,
                g_HowMuchLikeText,
                g_HowDifficultText,
                g_HowEnjoyableText,
                a_DescrptionText,
                a_ConfidentText,
                a_HowXWasThisText,
                a_HowMuchLikeText,
                a_HowDifficultText,
                a_HowEnjoyableText);
    }

}

 public static class ToggleGroupExtension
 {
     public static Toggle GetActive(this ToggleGroup aGroup)
     {
         return aGroup.ActiveToggles().FirstOrDefault();
     }
 }