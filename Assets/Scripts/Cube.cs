using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Cube : MonoBehaviour
{
    [SerializeField] private bool _isCollected = false;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _movingToSpeed;
    [SerializeField] private Point _point;

    private BoxCollider _boxCollider;
    private Renderer _renderer;
    private Coroutine _movingTo;
    private Coroutine _correctingPosition;
    private bool _firstTimeCollision = true;
    private bool _needToMove = false;

    public Point Point => _point;

    public bool IsCollected => _isCollected;
    public bool FirstTimeCollision => _firstTimeCollision;
    public Renderer Renderer => _renderer;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (_needToMove)
        {
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }
    }

    public void SetCollided()
    {
        _isCollected = true;
    }


    public void SetNoFirstCollision()
    {
        _firstTimeCollision = false;
    }

    public void SetNoTrigger()
    {
        if (_boxCollider.isTrigger)
        {
            _boxCollider.isTrigger = false;
        }
    }

    public void NeedMove(Player player)
    {
        _needToMove = true;
       _movingTo=StartCoroutine(MovingTo(player));
    }

    private IEnumerator MovingTo(Player player)
    {

        transform.rotation = player.transform.rotation;
        transform.position= new Vector3(transform.position.x, transform.position.y,player.transform.position.z+transform.localScale.z);
        while (transform.position.x!= player.transform.position.x)
        {
            Vector3 newPosition = new Vector3(player.transform.position.x, transform.position.y,transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, _movingToSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void CorrectPosition(Player player)
    {
        _correctingPosition = StartCoroutine(CorrectingPosition(player));
    }

    private IEnumerator CorrectingPosition(Player player)
    {
        while (transform.position.x != player.transform.position.x)
        {
            Vector3 newPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, _movingToSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void StopMove()
    {
        _needToMove = false;
        StopCoroutine(_movingTo);
    }

    public void Rotate()
    {
        StartCoroutine(Rotating());
    }

    public void ChangeColor(Color changerColor)
    {
        StartCoroutine(ChangingColor(changerColor));

    }

    private IEnumerator Rotating()
    {
        while (transform.localEulerAngles.y<90)
        {
            transform.Rotate(Vector3.up*90*_rotateSpeed*Time.deltaTime);
            yield return null;
        }
    }
    
    private IEnumerator ChangingColor(Color changerColor)
    {
        while (true)
        {
            _renderer.material.color = Color.Lerp(_renderer.material.color,changerColor,_rotateSpeed*Time.deltaTime);
            yield return null;
        }
    }
}
