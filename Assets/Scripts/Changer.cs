using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changer : MonoBehaviour
{
    [SerializeField ]private Renderer _renderer;

    public Color Color()
    {
        return _renderer.sharedMaterial.color;
    }
}
