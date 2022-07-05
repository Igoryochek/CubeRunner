using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Material))]
public class Player : MonoBehaviour
{
    [SerializeField] private List<Cube> _cubes;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private CameraMovement _camera;

    public Renderer Renderer => _renderer;
    public IReadOnlyList<Cube> Cubes => _cubes;

    public void AddCube(Cube cube)
    {
        _cubes.Add(cube);
    }
    
    public void RemoveCube(Cube cube)
    {
        _cubes.Remove(cube);
        cube.transform.SetParent(null);
    }

    public void CorrectCamera()
    {
        _camera.NeedToCenter();
    }
}
