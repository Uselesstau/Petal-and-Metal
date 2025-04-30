using UnityEngine;
using UnityEngine.EventSystems;

public class LevelText_Cs : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int levelIndex;
    private LevelPreview_Cs levelPreview;

    private bool mouse_over;

    void Start()
    {
        levelPreview = GameObject.Find("LevelPreview").GetComponent<LevelPreview_Cs>();
    }

    void Update()
    {
        if (mouse_over)
        {
            levelPreview.PreviewLevel(levelIndex);
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
    }
}
