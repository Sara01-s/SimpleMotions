using UnityEngine;

public class SliderValue : MonoBehaviour {

    [SerializeField] private RectTransform _rectTransform;

    public float GetHorizontalPosition() {
        print(_rectTransform.anchoredPosition.x);
        return _rectTransform.anchoredPosition.x;
    }

}