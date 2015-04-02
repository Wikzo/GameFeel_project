using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour
{

    public AudioClip CollectSound;
    private float _lerpTime = 0.25f;
    public GameObject ParticleEmitter;

    private AudioSource _audioSource;
    private BoxCollider2D _boxCollider;
    private Transform _transform;

    public Vector3 _startPos;
    private Vector3 _endPos;
    private float _currentLerpTime;
    private bool _lerp;
    private bool _lerpDone;
    private int _myIndex;

    public void Restart()
    {
        _lerp = false;
        ParticleEmitter.SetActive(true);
        _myIndex = 0;
        _lerpDone = false;
        _boxCollider.enabled = true;

        _transform.position = _startPos;
    }

    // Use this for initialization
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _transform = gameObject.transform;
        _startPos = transform.position;

        Restart();
    }

    // Update is called once per frame
    private void Update()
    {

        if (_lerpDone)
        {
            _transform.position = StateManager.Instance.StarsIcons[_myIndex].transform.position;
            return;
        }

        if (_lerp)
        {
            _currentLerpTime += Time.deltaTime;

            if (_currentLerpTime > _lerpTime)
            {
                _currentLerpTime = _lerpTime;

                StateManager.Instance.StarsIcons[_myIndex].transform.renderer.enabled = false;
                _lerp = false;
                _lerpDone = true;
                _audioSource.PlayOneShot(CollectSound);
            }

            float perc = _currentLerpTime / _lerpTime;
            _transform.position = Vector3.Lerp(_startPos, _endPos, perc);
        }

    }

    public void CollectStar()//
    {
        if (!_lerp)
        {
            _boxCollider.enabled = false;

            _myIndex = StateManager.Instance.CollectedSoFar;
            _endPos = StateManager.Instance.StarsIcons[_myIndex].transform.position;


            StateManager.Instance.CollectedSoFar++;

            _lerp = true;

            ParticleEmitter.SetActive(false);
        }

    }
}
