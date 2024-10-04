using UnityEngine;
using System.IO;

namespace SimpleMotions {

	public sealed class JsonSerializer : ISerializer {

		private readonly bool _prettyPrint;

		public JsonSerializer() {}

		public JsonSerializer(bool prettyPrint) {
			_prettyPrint = prettyPrint;
		}

		public T Deserialize<T>(string filepath) {
			string dataToLoad = string.Empty;

			using (var stream = new FileStream(filepath, FileMode.Open)) {
				using (var streamReader = new StreamReader(stream)) {
					dataToLoad = streamReader.ReadToEnd();
				}
			}

			T loadedData;

			try {
				loadedData = JsonUtility.FromJson<T>(dataToLoad);
			}
			catch (System.Exception e) {
				throw new System.Exception("Error while trying to load data from JSON", e);
			}
			
			return loadedData;
		}

		public void Serialize<T>(T data, string filepath) {
			var dataToStore = JsonUtility.ToJson(data, prettyPrint: _prettyPrint);

			Directory.CreateDirectory(Path.GetDirectoryName(filepath));

			using (var stream = new FileStream(filepath, FileMode.Create)) {
				using (var streamWriter = new StreamWriter(stream)) {
					streamWriter.Write(dataToStore);
				}
			}
		}
		
	}
}