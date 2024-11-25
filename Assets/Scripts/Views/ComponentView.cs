using UnityEngine.UI;
using UnityEngine;

public abstract class ComponentView : MonoBehaviour {

    [SerializeField] protected Button _AddOrRemoveKeyframe;
    [SerializeField] protected Button _UpdateKeyframe;

    [SerializeField] protected Image _KeyframeImage;

    [SerializeField] protected GameObject _AddOrRemoveBlocker;
    [SerializeField] protected GameObject _Updateblocker;

    [SerializeField] protected EditorPainter _EditorPainter;

    [SerializeField] protected Sprite _Add;
    [SerializeField] protected Sprite _Remove;
    [SerializeField] protected Sprite _Unchangeable;
    [SerializeField] protected Image _Update;

    protected bool _FrameHasKeyframe;

}