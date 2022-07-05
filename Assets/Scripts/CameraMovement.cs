using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _xOffset;
    [SerializeField] private float _yOffset;
    [SerializeField] private float _zOffset;
    [SerializeField] private float _speed;

    private bool _needToCenter = false;

    private void Update()
    {
        if (_needToCenter==false)
        {
            transform.position = new Vector3(_player.transform.position.x + _xOffset, _player.transform.position.y / 1.5f + _yOffset, _player.transform.position.z - _zOffset);
        }
    }

    public void NeedToCenter()
    {
        _needToCenter = true;
        StartCoroutine(MovingToCenter());
    }

    private IEnumerator MovingToCenter()
    {
        Vector3 newPosition = new Vector3(_player.transform.position.x + _xOffset / 1.5f, _player.transform.position.y / 1.5f + _yOffset, _player.transform.position.z - _zOffset);
        while (transform.position.x!=newPosition.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition,_speed*Time.deltaTime);
            yield return null;
        }
    }
}
