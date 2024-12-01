using SimpleMotions;
using UnityEngine;
using TMPro;

public class EditKeyframeView : MonoBehaviour {

    [SerializeField] private TMP_InputField _frame;
    [SerializeField] private TMP_Dropdown _easeDropdown;
    
    private int _entityId;
    private int _originalKeyframeEase;
    private int _originalKeyframeFrame;
    private ComponentDTO _keyframeComponent;

	private IEditKeyframeViewModel _editKeyframeViewModel;

    public void Configure(IEditKeyframeViewModel editKeyframeViewModel, IVideoTimelineViewModel videoTimelineViewModel) {        
		editKeyframeViewModel.UpdateKeyframeFrame.Subscribe(updatedFrame => {
			_originalKeyframeFrame = updatedFrame;
		});

        _frame.onSubmit.AddListener(input => {
            if (!int.TryParse(input, out var targetFrame)) {
                return;
            }

            if (targetFrame < 0 || targetFrame > videoTimelineViewModel.TotalFrameCount) {
    			Debug.Log("Keyframe can't be moved to a frame that do not exists.");
                return;
            }

			var keyframeDTO = new KeyframeDTO (_entityId, _keyframeComponent, _originalKeyframeFrame, _originalKeyframeEase);
            editKeyframeViewModel.NewKeyframeFrame.Value = (keyframeDTO, targetFrame);
        });

		_editKeyframeViewModel = editKeyframeViewModel;
        gameObject.SetActive(false);
    }

	public void OpenEditKeyframePanel(KeyframeDTO keyframeDTO, System.Action<int> resultCallback) {
		SetDisplayData(keyframeDTO);

		_frame.interactable = keyframeDTO.Frame != 0;
		
		_easeDropdown.onValueChanged.AddListener(newEase => {
			var keyframeDTO = new KeyframeDTO (_entityId, _keyframeComponent, _originalKeyframeEase, _originalKeyframeEase);
            _editKeyframeViewModel.NewKeyframeEase.Value = (keyframeDTO, newEase);

			resultCallback(newEase);
        });

        gameObject.SetActive(true);
	}

    public void SetDisplayData(KeyframeDTO keyframeDTO) {
        _originalKeyframeEase = keyframeDTO.Ease;
        _originalKeyframeFrame = keyframeDTO.Frame;

        _entityId = keyframeDTO.EntityId;
        _frame.text = keyframeDTO.Frame.ToString();
        _easeDropdown.value = keyframeDTO.Ease;
        _keyframeComponent = keyframeDTO.ComponentDTO;
    }

	private void OnDisable() {
		_easeDropdown.onValueChanged.RemoveAllListeners();
		_frame.onValueChanged.RemoveAllListeners();
	}

}