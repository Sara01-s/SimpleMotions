using UnityEngine.EventSystems;
using SimpleMotions;
using UnityEngine;
using TMPro;

public class KeyframeSelector : MonoBehaviour, IPointerDownHandler {

    private EditKeyframeView _editKeyframePanel;
    private ComponentDTO _componentDTO;
    private int _ownerEntityId;
    private int _frame;
    private int _ease;

    public void Configure(int ownerEntityId, ComponentDTO componentDTO, int frame, int ease, EditKeyframeView editKeyframePanel) {
        _editKeyframePanel = editKeyframePanel;
        _ownerEntityId = ownerEntityId;
        _componentDTO = componentDTO;
        _frame = frame;
        _ease = ease;
    }

    public void OnPointerDown(PointerEventData eventData) {
		var keyframeDTO = new KeyframeDTO(_ownerEntityId, _componentDTO, _frame, _ease);
		_editKeyframePanel.OpenEditKeyframePanel(keyframeDTO, OnEditResult);

		void OnEditResult(int newEase) {
			_ease = newEase;
		}
    }

}