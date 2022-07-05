using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatio : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        
    }

    private void Flip()
    {
        _animator.SetTrigger("Flip");
    }
    
    private void Celebrate()
    {
        _animator.SetTrigger("Victory");
    }
}
