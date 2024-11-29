using System;
using System.Collections.Generic;

#nullable enable

namespace SimpleMotions {

	public class ReactiveCommand<T1, T2> : IDisposable {

		private Action<T1, T2>? _onExecute = delegate {};
		private readonly List<Action<T1, T2>> _callbacks = new();

		public void Subscribe(Action<T1, T2> callback) {
			if (callback == null) {
				throw new ArgumentNullException(nameof(callback));
			}

			if (_callbacks.Contains(callback)) {
				return;
			}

			_onExecute += callback;
			_callbacks.Add(callback);
		}

		public void Unsubscribe(Action<T1, T2> callback) {
			if (!_callbacks.Contains(callback)) {
				throw new ArgumentException($"Callback {callback} not found in reactive command.");
			}

			_callbacks.Remove(callback);
			_onExecute -= callback;
		}

		public void Execute(T1 value1, T2 value2) {
			_onExecute?.Invoke(value1, value2);
		}

		public void Dispose() {
			_onExecute ??= delegate {};

			foreach (var callback in _onExecute.GetInvocationList()) {
				_onExecute -= (Action<T1, T2>)callback;
			}

			_callbacks.Clear();
			_onExecute = delegate {};
		}
	}

	public class ReactiveCommand<T> : IDisposable {

		private Action<T>? _onExecute = delegate {};
		private readonly List<Action<T>> _callbacks = new();

		public void Subscribe(Action<T> callback) {
			if (callback == null) {
				throw new ArgumentNullException(nameof(callback));
			}

			if (_callbacks.Contains(callback)) {
				return;
			}

			_onExecute += callback;
			_callbacks.Add(callback);
		}

		public void Unsubscribe(Action<T> callback) {
			if (!_callbacks.Contains(callback)) {
				throw new ArgumentException($"Callback {callback.Method.Name} not found in reactive command.");
			}

			_callbacks.Remove(callback);
			_onExecute -= callback;
		}

		public void Execute(T value) {
			_onExecute?.Invoke(value);
		}

		public void Dispose() {
			_onExecute ??= delegate {};

			foreach (var callback in _onExecute.GetInvocationList()) {
				_onExecute -= (Action<T>)callback;
			}

			_callbacks.Clear();
			_onExecute = delegate {};
		}
	}

	public class ReactiveCommand : IDisposable {

		private Action? _onExecute = delegate {};
		private readonly List<Action> _callbacks = new();

		public void Subscribe(Action callback) {
			if (callback == null) {
				throw new ArgumentNullException(nameof(callback));
			}

			if (_callbacks.Contains(callback)) {
				return;
			}

			_onExecute += callback;
			_callbacks.Add(callback);
		}

		public void Unsubscribe(Action callback) {
			if (!_callbacks.Contains(callback)) {
				throw new ArgumentException($"Callback {callback.Method.Name} not found in reactive command.");
			}

			_callbacks.Remove(callback);
			_onExecute -= callback;
		}

		public void Execute() {
			_onExecute?.Invoke();
		}

		public void Dispose() {
			_onExecute ??= delegate {};

			foreach (var callback in _onExecute.GetInvocationList()) {
				_onExecute -= (Action)callback;
			}

			_callbacks.Clear();
			_onExecute = delegate {};
		}
	}

}
