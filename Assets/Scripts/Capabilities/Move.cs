using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float speed = 0.1f;
    private Slide _slide;

    private void Awake()
    {
        _slide = GetComponent<Slide>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_slide.IsSliding() || _slide.IsFalling())
            return;
        var t = transform.position;
        t.y += speed * Time.deltaTime;
        transform.position = t;
    }
}