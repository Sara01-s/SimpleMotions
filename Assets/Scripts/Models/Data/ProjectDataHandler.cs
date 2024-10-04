namespace SimpleMotions {

	public sealed class ProjectDataHandler {

		private readonly DataSerializer<ProjectData> _projectDataSerializer;
		private readonly string _projectDataFilePath;

		public ProjectDataHandler(DataSerializer<ProjectData> projectDataSerializer, string projectDataFilePath) {
			_projectDataSerializer = projectDataSerializer;
			_projectDataFilePath = projectDataFilePath;
		}

		public void SaveData(ProjectData projectData) {
			_projectDataSerializer.SaveToFile(projectData, _projectDataFilePath);
		}

		public ProjectData LoadData() {
			return _projectDataSerializer.LoadFromFile(_projectDataFilePath)
				?? GetDefaultProjectData();
		}

		public ProjectData GetDefaultProjectData() {
			return new ProjectData();
		}

	}
}