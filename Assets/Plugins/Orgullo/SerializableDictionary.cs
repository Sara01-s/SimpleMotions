using System.Collections.Generic;
using UnityEngine;

namespace SimpleMotions {

    [System.Serializable]
    /// <summary> since Dictionaries cannot be serialized, you can use this class instead. </summary>
    public sealed class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver {
        [SerializeField] private List<TKey> _keys = new();
        [SerializeField] private List<TValue> _values = new();

        public void OnBeforeSerialize() {
			// Before serialize (save to file), save the dictionary as two lists.
            _keys.Clear();
            _values.Clear();

            foreach (var pair in this) {
                _keys.Add(pair.Key);
                _values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize() {
			// After deserialized (load from file), take the two lists and create a dictionary with them.
            this.Clear();

            for (int i = 0; i < Mathf.Min(_keys.Count, _values.Count); ++i) {
                this.Add(_keys[i], _values[i]);
            }
        }

    }
}