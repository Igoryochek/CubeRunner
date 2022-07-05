using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedChangingColor;
    [SerializeField] private float _sideSpeed;
    [SerializeField] private float _moveUpSpeed;
    [SerializeField] private float _upperLimitX;
    [SerializeField] private float _lowerLimitX;
    [SerializeField] private float _removingCubeDelay;
    [SerializeField] private ParticleSystem _firework;
    [SerializeField] private GameObject _levelComplete;
    [SerializeField] private GameObject _continueButton;

    private Player _player;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private Coroutine _movingUp;
    private Coroutine _removingLastCube;
    private bool _needCollide = true;

    private const string Win = "Win";
    private const string Flip = "Flip";

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();

    }

    private void Update()
    {

            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                Vector3 newPosition = new Vector3(raycastHit.point.x, transform.position.y, transform.position.z);
                transform.position = Vector3.LerpUnclamped(transform.position, newPosition, _sideSpeed * Time.deltaTime);
                if (transform.position.x > _upperLimitX)
                {
                    transform.position = new Vector3(_upperLimitX, transform.position.y, transform.position.z);
                }
                if (transform.position.x < _lowerLimitX)
                {
                    transform.position = new Vector3(_lowerLimitX, transform.position.y, transform.position.z);
                }

            }
        }
    }

    private void MoveUp(Cube cube)
    {
        if (_movingUp == null)
        {
            StartCoroutine(MovingUp(cube));
        }
    }

    private IEnumerator MovingUp(Cube cube)
    {
        _rigidbody.isKinematic = true;
        float newPositionY = (transform.position.y-_player.Cubes[_player.Cubes.Count-1].transform.position.y)+cube.Point.transform.position.y;
        while (transform.position.y!=newPositionY)
        {
            Vector3 newPosition = new Vector3(transform.position.x,newPositionY,cube.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, _moveUpSpeed * Time.deltaTime);
            yield return null;
        }
        CollectCube(cube);
        _rigidbody.isKinematic = false;

    }

    private void CollectCube(Cube cube)
    {
        cube.transform.SetParent(transform);
        cube.StopMove();
        cube.SetNoTrigger();
        cube.CorrectPosition(_player);
        _player.AddCube(cube);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Cube cube) && cube.Renderer.material.color == _player.Renderer.material.color)
        {
            if (cube.IsCollected == false)
            {
                cube.SetCollided();
                cube.NeedMove(_player);
                MoveUp(cube);
            }
        }
        if (other.TryGetComponent(out Cube cube1)&&cube.Renderer.sharedMaterial.color != _player.Renderer.sharedMaterial.color && cube.IsCollected == false&&_needCollide)
        {
            cube.SetCollided();
            RemoveLastCube();
            if (_player.Cubes.Count < 1)
            {
                _speed=0;
            }
        }
        if (other.TryGetComponent(out Changer changer))
        {
            if (changer.Color()!= _player.Renderer.sharedMaterial.color)
            {
                ChangeColor(changer.Color());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Obstacle obstacle)&& _player.Cubes.Count>1)
        {
            _player.RemoveCube(_player.Cubes[_player.Cubes.Count - 1]);
            if (_player.Cubes.Count < 1)
            {
                _speed = 0;
            }
        }
        
        if (collision.gameObject.TryGetComponent(out Step step)&& _player.Cubes.Count>=1)
        {
            _player.RemoveCube(_player.Cubes[_player.Cubes.Count - 1]);
            if (_player.Cubes.Count < 1)
            {
                _player.CorrectCamera();
                _speed = 0;
                _animator.SetTrigger(Win);
                _firework.Play();
                StartCoroutine(UIStarting());
            }
        } 
    }

    private IEnumerator UIStarting()
    {
        yield return new WaitForSeconds(4);
        _levelComplete.SetActive(true);
        _continueButton.SetActive(true);
    }

    private void ChangeColor(Color changerColor)
    {
        _player.Renderer.material.color= changerColor;
        _animator.SetTrigger(Flip);
        StartCoroutine(ChangingColor(changerColor));
    }

    private IEnumerator ChangingColor(Color changerColor)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_speedChangingColor);
        int count = _player.Cubes.Count - 1;
        for (int i = count ; i >= 0 ; i--)
        {
            _player.Cubes[i].Rotate();
            _player.Cubes[i].ChangeColor(changerColor);
            yield return waitForSeconds;
        }
    }

    public void RemoveLastCube()
    {
        if (_removingLastCube == null)
        {
            StartCoroutine(RemovingLastCube());
        }
    }

    private IEnumerator RemovingLastCube()
    {
        _needCollide = false;
        _player.Cubes[_player.Cubes.Count - 1].transform.SetParent(null);
        _player.RemoveCube(_player.Cubes[_player.Cubes.Count - 1]);
        yield return new WaitForSeconds(_removingCubeDelay);
        _needCollide = true;
        _removingLastCube = null;
    }
}
