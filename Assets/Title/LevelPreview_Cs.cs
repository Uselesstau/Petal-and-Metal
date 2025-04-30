using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPreview_Cs : MonoBehaviour
{
    public List<Sprite> levels;
    public Image levelPreview;

    public void PreviewLevel(int level)
    {
        levelPreview.sprite = levels[level-1];
    }
}
