namespace SimpleMotions {

	interface IEventService {

		void Subscribe<T>(System.Action<T> callback);
		void Unsubscribe<T>(System.Action<T> callback);
		void Dispatch<T>(T signal);

	}
}