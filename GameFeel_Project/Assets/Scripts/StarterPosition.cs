using UnityEngine;
using System.Collections;

public class StarterPosition : MonoBehaviour
{
    private Player _myPlayer;
    private Transform _playerTransform;
    private Transform _myTransform;

    private bool _hasSatCheckpoint = false;
    // Use this for initialization
    private void Start()
    {
        _myPlayer = ParameterManager.Instance.Player;
        _playerTransform = _myPlayer.transform;
        _myTransform = gameObject.transform;
    }

    public void Restart()
    {
        _hasSatCheckpoint = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_hasSatCheckpoint)
            return;

        if (_playerTransform.position.x > _myTransform.position.x)
        {
            _hasSatCheckpoint = true;
            _myPlayer.StartPosition = _myTransform.position;
        }
    }
}
