using System.Collections.Generic;
using System;

namespace SimpleMotions {

    public sealed class ReactiveValue<T> {

        public T Value { 
            get {
                return _value;
            } 
            set {
				_value = value;
				OnValueChanged?.Invoke(_value);
            } 
        }

        private event Action<T> OnValueChanged;
        private T _value;
        private readonly List<Action<T>> _callbacks = new();

		public void Subscribe(Action<T> value, bool invokeImmediately = false) {
			if (value != null) {
				OnValueChanged += value;
				_callbacks.Add(value);

				if (invokeImmediately) {
					value(_value);
				}
			}
		}

        public void Dispose() {
            foreach (var action in _callbacks) {
                OnValueChanged -= action;
            }

            _callbacks.Clear();
            OnValueChanged = null;
        }

    }
}