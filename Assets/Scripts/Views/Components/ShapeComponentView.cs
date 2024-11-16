using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;

public class ShapeComponentView : MonoBehaviour {

    [SerializeField] private GameObject[] _entitiesUI;

    private EditorPainter _editorPainter;

    public void Configure(IShapeComponentViewModel shapeComponentViewModel) { }

    public void RefreshData(((float r, float g, float b, float a) color, string primitiveShape) shapeData, EditorPainter editorPainter) {
        _editorPainter = editorPainter;

        UpdateShape(shapeData.primitiveShape);
    }

    private void UpdateShape(string primitiveShape) {
        foreach (var entityUI in _entitiesUI) {
            var shapeType = entityUI.GetComponent<ShapeType>().ShapeTypeUI;

            if (shapeType.ToString() == primitiveShape) {
                entityUI.GetComponent<Image>().color = _editorPainter.Theme.AccentColor;
            }
        }
    }

}