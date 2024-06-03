using System.Collections.Generic;
using System;

namespace SimpleMotions {

    public sealed class ReactiveValue<T> {

        internal T Value { 
            get {
                return _value;
            } 
            set {
                _value = value;
                OnValueChanged?.Invoke(_value);
            } 
        }

        private Action<T> OnValueChanged;
        private T _value;
        private readonly List<Action<T>> _callbacks = new();

        internal void Subscribe(Action<T> value) {
            if (value != null) {
                OnValueChanged += value;
                _callbacks.Add(value);
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