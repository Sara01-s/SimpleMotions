using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Image))]
public class PerObjectMotionBlur : MonoBehaviour {

    [field:SerializeField, Range(0.0f, 1.0f)] 
	public float BlurAmount { get; private set; } = 0.5f;

    private Matrix4x4 _previousModelMatrix;
	private Material _motionBlurMaterial;
	private Image _image;

	private bool _shaderSet = false;

    private void Awake() {
		_image = GetComponent<Image>();
        _previousModelMatrix = transform.localToWorldMatrix;
    }

	public void SetShader(Shader shader) {
		_motionBlurMaterial = new Material(shader);
		_shaderSet = true;
	}

    private void LateUpdate() {
		if (!_shaderSet) {
			return;
		}

        Matrix4x4 currentModelMatrix = transform.localToWorldMatrix;
		
        _motionBlurMaterial.SetMatrix("_CurrentMatrix", currentModelMatrix);
        _motionBlurMaterial.SetMatrix("_PreviousMatrix", _previousModelMatrix);
        _motionBlurMaterial.SetFloat("_BlurAmount", BlurAmount);

		_image.material = _motionBlurMaterial;

        _previousModelMatrix = currentModelMatrix;
    }
}
