using SimpleMotions;
using UnityEngine;
using TMPro;

public class EditKeyframeView : MonoBehaviour {

    [SerializeField] private TimelineView _timelineView;
    [SerializeField] private TMP_InputField _frame;
    [SerializeField] private TMP_Dropdown _easeDropdown;
    
    private int _entityid;
    private int _originalKeyframeEase;
    private int _originalKeyframeFrame;

    private ComponentDTO _keyframeComponent;

    public void Configure(IEditKeyframeViewModel editKeyframeViewModel, IVideoTimelineViewModel videoTimelineViewModel) {        
        _frame.onSubmit.AddListener(input => {
            if (!int.TryParse(input, out var targetFrame)) {
                return;
            }

            if (targetFrame < 0 || targetFrame > videoTimelineViewModel.TotalFrameCount) {
                return;
            }

            //if (_originalKeyframeFrame == 0 && targetFrame == 0) {
            //    return;
            //}
            
            editKeyframeViewModel.NewKeyframeFrame.Value = (_entityid, _originalKeyframeFrame, targetFrame);
        });

        _easeDropdown.onValueChanged.AddListener(newEase => {
            editKeyframeViewModel.NewKeyframeEase.Value = (_entityid, _originalKeyframeEase, newEase);
        });

        gameObject.SetActive(false);
    }

    public void SetData(int id, ComponentDTO componentDTO, int frame, int ease) {
        _originalKeyframeEase = ease;
        _originalKeyframeFrame = frame;

        _entityid = id;
        _frame.text = frame.ToString();
        _easeDropdown.value = ease;

        //_keyframeComponent = componentDTO;
    }

}