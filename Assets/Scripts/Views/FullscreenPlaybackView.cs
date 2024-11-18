using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class FullscreenPlaybackView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public bool IsFullscreen;

    [SerializeField] private RectTransform _playbackView;
    [SerializeField] private float _timeToHide = 2.0f;
    [SerializeField] private float _timeToStartHiding = 2.0f;

    private readonly List<Image> _images = new();
    private readonly List<TextMeshProUGUI> _texts = new();

    private Coroutine _waitToHidePlayback;
    private bool _stopFade;

    public void Configure() {
        if (_playbackView.TryGetComponent<Image>(out var image)) {
            _images.Add(image);
        }

        foreach (RectTransform majorChild in _playbackView) {
            if (majorChild.TryGetComponent<Image>(out var majorImage)) {
                _images.Add(majorImage);
            }

            if (majorChild.TryGetComponent<TextMeshProUGUI>(out var majorText)) {
                _texts.Add(majorText);
            }

            foreach (RectTransform mediumChild in majorChild) {
                if (mediumChild.TryGetComponent<Image>(out var mediumImage)) {
                    _images.Add(mediumImage);
                }

                foreach (RectTransform minorChild in mediumChild) {
                    if (minorChild.TryGetComponent<TextMeshProUGUI>(out var minorText)) {
                        _texts.Add(minorText);
                    }
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        StopCoroutine(_waitToHidePlayback);

        if (IsFullscreen) {
            _stopFade = true;

            foreach (var image in _images) {
                image.CrossFadeAlpha(1.0f, 0.0f, false);
            }

            foreach (var text in _texts) {
                text.CrossFadeAlpha(1.0f, 0.0f, false);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        _waitToHidePlayback = StartCoroutine(CO_WaitToHidePlayback());
    }

    private IEnumerator CO_WaitToHidePlayback() {
        while (IsFullscreen) {
            yield return new WaitForSeconds(_timeToStartHiding);

            if (_stopFade) {
                yield return null;
            }

            foreach (var image in _images) {
                image.CrossFadeAlpha(0.0f, _timeToHide, false);
            }

            foreach (var text in _texts) {
                text.CrossFadeAlpha(0.0f, _timeToHide, false);
            }
        }
    }

}