using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowControls : MonoBehaviour
{

    private bool _showControls;
    public GameObject TextObject;

    public Slider MySlider;
    public Text SolverText;

    // Use this for initialization
    private void Start()
    {
        _showControls = false;

        MySlider.value = Physics.solverIterationCount;

        if (_showControls)
            TextObject.SetActive(true);
        else
            TextObject.SetActive(false);
    }

    public void OnToggle()
    {
        _showControls = !_showControls;

        if (_showControls)
            TextObject.SetActive(true);
        else
            TextObject.SetActive(false);
    }

    public void ChangeSolverCount()
    {
        int count = (int)MySlider.value;
        Physics.solverIterationCount = count;

        string s = string.Format("SolverIterationCount: {0}\n(requires restart)", count);

        SolverText.text = s;
    }
}