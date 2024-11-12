using UnityEngine;

public class FCP_ExampleScript : MonoBehaviour {

    public bool getStartingColorFromMaterial;
    public FlexibleColorPicker fcp;
    public Material material;

    private void Start() {
        if(getStartingColorFromMaterial)
            fcp.Color = material.color;

        fcp.OnColorChange.AddListener(OnChangeColor);
    }

    private void OnChangeColor(Color co) {
        material.color = co;
    }
}
