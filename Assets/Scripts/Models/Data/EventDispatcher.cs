using System;
using System.Collections.Generic;

namespace SimpleMotions {

    public sealed class EventDispatcher : IEventService, IDisposable {
		
        private readonly Dictionary<Type, List<Delegate>> _events;

        public EventDispatcher() {
            _events = new Dictionary<Type, List<Delegate>>();

			Services.Instance.RegisterService<IEventService>(service: this);
        }

        public void Subscribe<T>(Action<T> callback) {
            var type = typeof(T);

            if (!_events.ContainsKey(type)) {
                _events[type] = new List<Delegate>();
            }

            _events[type].Add(callback);
        }

        public void Unsubscribe<T>(Action<T> callback) {
            var type = typeof(T);

            if (_events.ContainsKey(type)) {
                _events[type].Remove(callback);
            }
        }

        public void Dispatch<T>(T signal) {
            var type = typeof(T);

            if (_events.ContainsKey(type)) {
                foreach (var callback in _events[type]) {
                    ((Action<T>)callback).Invoke(signal);
                }
            }
        }

		public void Dispose() {
			Services.Instance.UnRegisterService<IEventService>();
		}

	}
}
