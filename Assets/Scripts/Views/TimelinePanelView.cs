using UnityEngine;
using UnityEngine.UI;
using SimpleMotions;

public class TimelinePanelView : MonoBehaviour {

	[SerializeField] private RectTransform _entityHierarchyContent;
	[SerializeField] private GameObject _entityPanelPrefab;
	[SerializeField] private GameObject _maxEntitiesWarning;
	[SerializeField] private Button _createEntity;

	private const float START_ENTITY_PANELS_SUPPORT = 3.0f;
	private GridLayoutGroup _hierarchyGrid;

	public void Configure(ITimelinePanelViewModel timelinePanelViewModel, IEntitySelectorViewModel entitySelectorViewModel) {
		timelinePanelViewModel.ShowMaxEntitiesWarning.Subscribe(ShowMaxEntitiesWarning);
		timelinePanelViewModel.DeleteEntity.Subscribe(_ => AdjustHierarchyHeight(isAddingEntity: false));
		_hierarchyGrid = _entityHierarchyContent.GetComponent<GridLayoutGroup>();

		_createEntity.onClick.AddListener(() => {
			if (timelinePanelViewModel.TryCreateEntity(out int createdEntityId)) {
				var entityPanel = Instantiate(_entityPanelPrefab, parent: _entityHierarchyContent).GetComponent<EntityPanelView>();
				entityPanel.Configure(timelinePanelViewModel, entitySelectorViewModel, createdEntityId);

				AdjustHierarchyHeight(isAddingEntity: true);
			}
		});

		float hierarchyContentHeight = _hierarchyGrid.cellSize.y * START_ENTITY_PANELS_SUPPORT; // Prepare height for 4 entity panels.
		_entityHierarchyContent.sizeDelta = new Vector2(_entityHierarchyContent.sizeDelta.x, hierarchyContentHeight);
	}

	private void AdjustHierarchyHeight(bool isAddingEntity) {
		if (_hierarchyGrid.transform.childCount >= 50) { // SHHHHHHHHHHHHHHHHHHHHHHHH // TODO - REMOVE THIS FROM EXISTANCE
			_entityHierarchyContent.sizeDelta += new Vector2(0.0f, 10.0f);
			return;
		}

		if (_hierarchyGrid.transform.childCount > START_ENTITY_PANELS_SUPPORT) {
			int multiplier = System.Convert.ToInt32(isAddingEntity);
			_entityHierarchyContent.sizeDelta += new Vector2(0.0f, _hierarchyGrid.cellSize.y + _hierarchyGrid.spacing.y * multiplier);
		}
	}

	private void ShowMaxEntitiesWarning() {
		_maxEntitiesWarning.SetActive(true);
	}
}