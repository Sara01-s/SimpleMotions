using UnityEngine.EventSystems;
using SimpleMotions;
using UnityEngine;
using TMPro;

public class KeyframeSelector : MonoBehaviour, IPointerDownHandler {

    public int Ease { get; set; }

    private GameObject _editKeyframePanel;
    private int _ownerEntityId;
    private ComponentDTO _componentDTO;
    private int _frame;

    public void Configure(int ownerEntityId, ComponentDTO componentDTO, int frame, int ease, GameObject editKeyframePanel) {
        _editKeyframePanel = editKeyframePanel;
        _ownerEntityId = ownerEntityId;
        _componentDTO = componentDTO;
        _frame = frame;
        Ease = ease;
    }

    public void OnPointerDown(PointerEventData eventData) {
        var editKeyframeView = _editKeyframePanel.GetComponent<EditKeyframeView>();
        editKeyframeView.SetData(_ownerEntityId, _componentDTO, _frame, Ease);

        var frameInputField = _editKeyframePanel.GetComponentInChildren<TMP_InputField>();

		frameInputField.interactable = _frame != 0;
        _editKeyframePanel.SetActive(true);
    }

}