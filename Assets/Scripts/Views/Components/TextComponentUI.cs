using TMPro;
using UnityEngine;

public class TextComponentUI : MonoBehaviour {

    [SerializeField] private TMP_InputField _value;

    public void RefreshData(string text) {
        _value.text = text;
    }

}