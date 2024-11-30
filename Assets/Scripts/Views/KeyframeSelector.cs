using UnityEngine.EventSystems;
using SimpleMotions;
using UnityEngine;
using TMPro;

public class KeyframeSelector : MonoBehaviour, IPointerDownHandler {

    private int _ownerEntityId;
    private ComponentDTO _componentDTO;
    private int _frame;
    private int _ease;

    private GameObject _editKeyframePanel;

    public void Configure(int ownerEntityId, ComponentDTO componentDTO, int frame, int ease, GameObject editKeyframePanel) {
        _ownerEntityId = ownerEntityId;
        _componentDTO = componentDTO;
        _frame = frame;
        _ease = ease;
        _editKeyframePanel = editKeyframePanel;
    }

    public void OnPointerDown(PointerEventData eventData) {
        var editKeyframeView = _editKeyframePanel.GetComponent<EditKeyframeView>();
        editKeyframeView.SetData(_ownerEntityId, _componentDTO, _frame, _ease);

        var frameInputField = _editKeyframePanel.GetComponentInChildren<TMP_InputField>();

        if (_frame == 0) {
            frameInputField.interactable = false;
        }
        else {
            frameInputField.interactable = true;
        }
        
        _editKeyframePanel.SetActive(true);
    }

}