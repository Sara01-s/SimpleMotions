using SimpleMotions;
using UnityEngine;

public class EntitySelectorView : MonoBehaviour {

	[SerializeField] private Canvas _editorCanvas;
	[SerializeField] private Transform _canvasOrigin;
	[SerializeField] private RectTransform _canvasArea;
	[SerializeField] private RectTransform _selectionGizmo;
	[SerializeField] private Vector2 _selectionGizmoMargin;
	[SerializeField] private SelectionGizmo[] _selectionGizmoParts;

	private IEntitySelectorViewModel _entitySelectorViewModel;
	private IVideoPlaybackViewModel _videoPlaybackViewModel;
	private Camera _editorCamera;

	private bool _isExporting;

	public void Configure(IEntitySelectorViewModel entitySelectorViewModel, IVideoCanvasViewModel videoCanvasViewModel, 
						  IFullscreenViewModel fullscreenViewModel, IVideoPlaybackViewModel videoPlaybackViewModel, IExportViewModel exportViewModel) 
	{
		entitySelectorViewModel.OnEntitySelected.Subscribe(DrawSelectionGizmoOverEntity);
		entitySelectorViewModel.OnEntityDeselected.Subscribe(HideSelectionGizmo);

		videoCanvasViewModel.OnCanvasUpdate.Subscribe(entityDTO => {
			if (_entitySelectorViewModel.HasSelectedEntity) {
				DrawSelectionGizmoOverEntity(entityDTO);
			}
		});

		foreach (var selectionGizmoPart in _selectionGizmoParts) {
			selectionGizmoPart.Configure(entitySelectorViewModel, _editorCanvas.worldCamera, (RectTransform)_editorCanvas.transform);
			selectionGizmoPart.OnEntityChanged.Subscribe(DrawSelectionGizmoOverEntity);
		}
		
		fullscreenViewModel.OnFullscreen.Subscribe(() => HideSelectionGizmo());
		exportViewModel.OnExportEnd.Subscribe(() => _isExporting = false);
		exportViewModel.OnExportStart.Subscribe(_ => { 
			_isExporting = true;
			HideSelectionGizmo();
		});


		_editorCamera = _editorCanvas.worldCamera;
		_entitySelectorViewModel = entitySelectorViewModel;
		_videoPlaybackViewModel = videoPlaybackViewModel;
	}

	private void HideSelectionGizmo() {
		_selectionGizmo.gameObject.SetActive(false);
	}

	private Vector2 ScreenToCanvasRect(Vector2 screenPos) {
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			_canvasArea, screenPos, _editorCanvas.worldCamera, 
			out var rectPos
		);

		return rectPos;
	}

	private void DrawSelectionGizmoOverEntity(EntityDTO entityDTO) {
		if (_videoPlaybackViewModel.IsPlaying.Value && !_entitySelectorViewModel.HasSelectedEntity) {
			return;
		}

		if (!_entitySelectorViewModel.TryGetEntityTransform(entityDTO.Id, out var transformDTO) || _isExporting) {
			return;
		}

		var entityLocalPos = new Vector2(transformDTO.Position.x, transformDTO.Position.y);
		var entityLocalExtents = new Vector2(transformDTO.Scale.w * 0.5f, transformDTO.Scale.h * 0.5f);

		var entityWorldBoundsMin = _canvasOrigin.TransformPoint(entityLocalPos - entityLocalExtents);
		var entityWorldBoundsMax = _canvasOrigin.TransformPoint(entityLocalPos + entityLocalExtents);

		var entityScreenBoundsMin = _editorCamera.WorldToScreenPoint(entityWorldBoundsMin);
		var entityScreenBoundsMax = _editorCamera.WorldToScreenPoint(entityWorldBoundsMax);

		var entityRectBoundsMin = ScreenToCanvasRect(entityScreenBoundsMin);
		var entityRectBoundsMax = ScreenToCanvasRect(entityScreenBoundsMax);

		var entityRectSize = entityRectBoundsMax - entityRectBoundsMin;
		var entityRectCenter = entityRectBoundsMin + (entityRectSize * 0.5f);

		_selectionGizmo.localRotation = Quaternion.identity;
		_selectionGizmo.sizeDelta = entityRectSize + _selectionGizmoMargin;
		_selectionGizmo.anchoredPosition = entityRectCenter;

		_selectionGizmo.localRotation = Quaternion.AngleAxis(transformDTO.RollDegrees, Vector3.forward);
		_selectionGizmo.gameObject.SetActive(true);
	}

}
