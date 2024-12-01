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

    private KeyframeSelector _keyframeSelector;

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

			var keyframeDTO = new KeyframeDTO (_keyframeComponent, _entityId, _originalKeyframeFrame, _originalKeyframeEase);
            editKeyframeViewModel.NewKeyframeFrame.Value = (keyframeDTO, targetFrame);
        });

        _easeDropdown.onValueChanged.AddListener(newEase => {
			var keyframeDTO = new KeyframeDTO (_keyframeComponent, _entityId, _originalKeyframeEase, _originalKeyframeEase);
            editKeyframeViewModel.NewKeyframeEase.Value = (keyframeDTO, newEase);
            
            _keyframeSelector.Ease = newEase;
        });

        gameObject.SetActive(false);
    }

    public void SetData(int id, ComponentDTO componentDTO, int frame, int ease) {
        _originalKeyframeEase = ease;
        _originalKeyframeFrame = frame;

        _entityId = id;
        _frame.text = frame.ToString();
        _easeDropdown.value = ease;
        _keyframeComponent = componentDTO;
    }

}