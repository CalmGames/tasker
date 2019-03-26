using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PanelDivider : MonoBehaviour
{
    public RectTransform scrollView;
    public RectTransform[] separators;

    private void Update()
    {
        float i = (scrollView.rect.width / 3) / 2;

        separators[0].position = new Vector3(i, 0, 0);
        separators[1].position = new Vector3(-i, 0, 0);
    }
}
