using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FeelExamples : MonoBehaviour
{

    private readonly string[] _examples = {"Floaty",
                                           "Fragile",
                                           "Stiff",
                                           "Rigid",
                                           "Firm",
                                           "Solid",
                                           "Thick",
                                           "Fixed",
                                           "Robust",
                                           "Sore",
                                           "Steadfast",
                                           "Wild",
                                           "Constant",
                                           "Free",
                                          "Hard",
                                          "Tough",
                                          "Restricted",
                                          "Limited",
                                          "Reduced",
                                          "Fast",
                                          "Heavy",
                                          "Slow",
                                          "Enjoyable",
                                          "Stressful",
                                          "Annoying",
                                          "Realistic",
                                          "Difficult",
                                          "Easy",
                                          "Dry",
                                          "Juicy",
                                          "Mechanical",
                                          "Automatic",
                                          "Organic",
                                          "Exciting",
                                          "Wet",
                                          "Simple",
                                          "Complictated",
                                          "Direct",
                                          "Inert",
                                          "Unrealistic",
                                          "Light",
                                          "Normal"

                                          };



    private Text _myText;
    // Use this for initialization
    private void Start()
    {
        _myText = GetComponent<Text>();

        List<int> randomNumbers = new List<int>();;
        int ex1, ex2, ex3, ex4;

        ex1 = Random.Range(0, _examples.Count());
        randomNumbers.Add(ex1);

        // make sure no repeats!
        do
        {
            ex2 = Random.Range(0, _examples.Count());

        } while (randomNumbers.Contains(ex2));

        randomNumbers.Add(ex2);


        do
        {
            ex3 = Random.Range(0, _examples.Count());

        } while (randomNumbers.Contains(ex3));

        randomNumbers.Add(ex3);

        do
        {
            ex4 = Random.Range(0, _examples.Count());

        } while (randomNumbers.Contains(ex4));

        string s1 = _examples[ex1];
        string s2 = _examples[ex2];
        string s3 = _examples[ex3];
        string s4 = _examples[ex4];

        string t = string.Format("Examples: {0}, {1}, {2}, {3} ", s1.ToLower(), s2.ToLower(), s3.ToLower(), s4.ToLower());

        _myText.text = t;
    }

}