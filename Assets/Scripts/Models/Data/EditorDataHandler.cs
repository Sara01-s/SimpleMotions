using SimpleMotions.Internal;

namespace SimpleMotions {

	public sealed class EditorDataHandler {

		private readonly DataSerializer<EditorData> _editorDataSerializer;
		private readonly string _editorDataFilePath;

		public EditorDataHandler(DataSerializer<EditorData> editorDataSerializer, string editorDataFilepath) {
			_editorDataSerializer = editorDataSerializer;
			_editorDataFilePath = editorDataFilepath;
		}

		public void SaveData(EditorData editorData) {
			_editorDataSerializer.SaveToFile(editorData, _editorDataFilePath);
		}

		public EditorData LoadData() {
			return _editorDataSerializer.LoadFromFile(_editorDataFilePath) 
				?? GetDefaultEditorData();
		}

		public EditorData GetDefaultEditorData() {
			return new EditorData();
		}

	}
}