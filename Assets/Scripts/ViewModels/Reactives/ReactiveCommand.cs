using System.Collections.Generic;
using System;

namespace SimpleMotions {

    public sealed class ReactiveCommand<T> : IDisposable {

        private Action<T> _onExecute;
        private readonly List<Action<T>> _callbacks = new();

        public void Subscribe(Action<T> callback) {
            if (callback != null) {
                _onExecute += callback;
                _callbacks.Add(callback);
            }
        }

        public void Execute(T value) {
            _onExecute?.Invoke(value);
        }

        public void Dispose() {
            foreach (var action in _callbacks) {
                _onExecute -= action;
            }

            _callbacks.Clear();
            _onExecute = null;
        }
        
    }
}