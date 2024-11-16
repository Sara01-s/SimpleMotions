using System;
using System.Collections.Generic;

namespace SimpleMotions {

	public class ReactiveCommand<T1, T2> : IDisposable {

        private Action<T1, T2> _onExecute;
        private readonly List<Action<T1, T2>> _callbacks = new();

        public void Subscribe(Action<T1, T2> callback) {
            if (callback != null) {
                _onExecute += callback;
                _callbacks.Add(callback);
            }
        }

        public void Execute(T1 value1, T2 value2) {
            _onExecute?.Invoke(value1, value2);
        }

        public void Dispose() {
            foreach (var action in _callbacks) {
                _onExecute -= action;
            }

            _callbacks.Clear();
            _onExecute = null;
        }
    }

    public class ReactiveCommand<T> : IDisposable {

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

    public class ReactiveCommand : ReactiveCommand<Void> {

        public void Execute() => base.Execute(Void.Nothing);

        public void Subscribe(Action callback) {
            base.Subscribe(_ => callback?.Invoke());
        }
    }

    public readonly struct Void {
		public static readonly Void Nothing = new();
	}
}
