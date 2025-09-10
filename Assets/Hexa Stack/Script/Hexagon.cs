using UnityEngine;
using System;
using System.Collections.Generic;
public class Hexagon : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private new Renderer renderer;
    [SerializeField] private new Collider collider;
    public HexStack HexStack {  get; private set; }
    public Color Color
    {
        get { return renderer.material.color; }
        set { renderer.material.color = value; }
    }

    public void Configure(HexStack hexStack)
    {
        this.HexStack = hexStack;
    }

    public void DisableCollider() => collider.enabled = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
