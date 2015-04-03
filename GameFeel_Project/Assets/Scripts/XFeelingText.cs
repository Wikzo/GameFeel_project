using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class XFeelingText : MonoBehaviour
{
    public Text InputFeeling;

    private Text _myText;
    // Use this for initialization
    private void Start()
    {
        _myText = GetComponent<Text>();
    }

    // Update is called once per frame
    private void Update()
    {
        _myText.text = InputFeeling.text;
    }
}
