using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffectUI_Cs : MonoBehaviour
{
    public IEnumerator FadeOut(float speed = 0.8f)
    {
        float a = 0f;
        while (a < 1.0f)
        {
            a += speed * Time.deltaTime;
            GetComponent<Image>().color = new Color(0f, 0f, 0f, a);
            yield return null;
        }
    }
}
