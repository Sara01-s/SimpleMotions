using SimpleMotions;
using UnityEngine;
using TMPro;

public class EditKeyframeView : MonoBehaviour {

    [SerializeField] private TimelineView _timelineView;
    [SerializeField] private TMP_InputField _frame;
    [SerializeField] private TMP_Dropdown _easeDropdown;
    
    private int _keyframeId;
    private int _currentKeyframeFrame;
    private ComponentDTO _keyframeComponent;


    public void Configure(IEditKeyframeViewModel editKeyframeViewModel, IVideoTimelineViewModel videoTimelineViewModel) {        
        _frame.onSubmit.AddListener(input => {
            if (!int.TryParse(input, out var frame)) {
                return;
            }

            if (frame < 0 || frame > videoTimelineViewModel.TotalFrameCount) {
                return;
            }

            if (_currentKeyframeFrame == 0 && frame == 0) {
                return;
            }
            
            editKeyframeViewModel.OnKeyframeEdit.Value = new KeyframeDTO(_keyframeComponent, _keyframeId, frame, _easeDropdown.value);;
        });

        _easeDropdown.onValueChanged.AddListener(newEase => {
            if (!int.TryParse(_frame.text, out var frame)) {
                return;
            }

            editKeyframeViewModel.OnKeyframeEdit.Value = new KeyframeDTO(_keyframeComponent, _keyframeId, frame, newEase);
        });
    }

    public void SetData(int id, ComponentDTO componentDTO, int frame, int ease) {
        _keyframeId = id;
        _keyframeComponent = componentDTO;
        _frame.text = frame.ToString();
        _easeDropdown.value = ease;

        _currentKeyframeFrame = frame;
    }

}