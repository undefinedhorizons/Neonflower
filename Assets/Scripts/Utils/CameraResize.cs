using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraResize : MonoBehaviour
{
    public int fullWidthUnits = 11;
    public float shift = .05f;

    void Start()
    {
        // Force fixed width
        var ratio = (float)Screen.height / Screen.width;
        GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = (fullWidthUnits - shift) * ratio / 2.0f;
    }
}