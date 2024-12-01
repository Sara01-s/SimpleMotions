using UnityEngine;

public class SliderValue : MonoBehaviour {

    [SerializeField] private RectTransform _rectTransform;

    public float GetHorizontalPosition() {
        return _rectTransform.anchoredPosition.x;
    }

}