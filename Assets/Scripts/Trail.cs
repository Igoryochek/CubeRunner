using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TrailRenderer _renderer;
    [SerializeField] private float _speed;

    private void Start()
    {
        StartCoroutine(Moving());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Changer changer))
        {
            _renderer.material.color = changer.Color();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Obstacle obstacle)|| collision.gameObject.TryGetComponent(out Step step))
        {
            transform.position = new Vector3(_player.transform.position.x,_player.Cubes[_player.Cubes.Count-1].transform.position.y-0.5f, _player.transform.position.z);
        }
    }

    private IEnumerator Moving()
    {
        while (true)
        {
            Vector3 newPosition = new Vector3(_player.transform.position.x,transform.position.y,_player.transform.position.z);
            transform.position =Vector3.MoveTowards(transform.position,newPosition,_speed*Time.deltaTime);
            yield return null;
        }
    }
}
