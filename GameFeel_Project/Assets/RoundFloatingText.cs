using UnityEngine;
using System.Collections;

public class RoundFloatingText : MonoBehaviour
{
    public Color ColorStart, ColorEnd;
    public float LerpTime = 1f;
    public float UpSpeed = 1f;

    private float currentLerpTime;

    private TextMesh _textMesh;
    // Use this for initialization
    private void Start()
    {
        _textMesh = GetComponent<TextMesh>();
        _textMesh.color = ColorStart;

        currentLerpTime = -2;

        _textMesh.text = string.Format("Round: {0} / {1}", (ParameterManager.Instance.Level + 1),
            ParameterManager.Instance.MyParameters.Count);

        transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }

    // Update is called once per frame
    private void Update()
    {

        currentLerpTime += Time.deltaTime;

        if (currentLerpTime > 0)
        {
            float percent = currentLerpTime/LerpTime;
            _textMesh.color = Color.Lerp(ColorStart, ColorEnd, percent);

        }

        transform.Translate(Vector3.up * UpSpeed * Time.deltaTime);

        //if (currentLerpTime > LerpTime)
          //  Destroy(gameObject);
    }
}
