using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjectFx : MonoBehaviour
{
    Material mat;
    Outline outline;

    void Awake()
    {
        mat = GetComponent<Renderer>().material;
        outline = GetComponent<Outline>();

        outline.OutlineWidth = 0f;
    }

    public void VisibilityFx (bool _value)
    {
        int result = _value ? 1 : 0;

        Color finalColor = Color.white;
        finalColor.a = (float)result;

        mat.SetColor("_Color", finalColor);
        mat.SetInt("_Fx", result);

        float with = _value ? 1.5f : 0f;
        outline.OutlineWidth = with;
    }
}
