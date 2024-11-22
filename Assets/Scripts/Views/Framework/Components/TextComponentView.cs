using SimpleMotions;
using UnityEngine;
using TMPro;

public class TextComponentView : MonoBehaviour {

    [SerializeField] private TMP_InputField _value;

    private ITextComponentViewModel _textComponentViewModel;

    public void Configure(ITextComponentViewModel textComponentViewModel) {
        _textComponentViewModel = textComponentViewModel;

        _value.onValueChanged.AddListener(SendText);
    }

    private void SendText(string value) {
        _textComponentViewModel.Text.Execute(value);
    }

    public void RefreshData(string text) {
        _value.text = text;
    }

}