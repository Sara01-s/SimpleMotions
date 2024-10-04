using System.Collections.Generic;
using UnityEngine;

namespace SimpleMotions {

    [System.Serializable]
    /// <summary> since Dictionaries cannot be serialized, you can use this class instead. </summary>
    public sealed class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver {
        [SerializeField] private List<TKey> _dictionaryKeys = new();
        [SerializeField] private List<TValue> _dictionaryValues = new();

        public void OnAfterDeserialize() {
            _dictionaryKeys.Clear();
            _dictionaryValues.Clear();

            foreach(var pair in this) {
                _dictionaryKeys.Add(pair.Key);
                _dictionaryValues.Add(pair.Value);
            }
        }

        public void OnBeforeSerialize() {
            this.Clear();

            if (_dictionaryKeys.Count != _dictionaryValues.Count) {
                Debug.LogError(
                    "Tried to deserialize a SerializableDictionary, but the amount of keys (" + 
                    _dictionaryKeys.Count + ") does not match the number of values (" + 
                    _dictionaryValues.Capacity + ")");
            }

            for (int i = 0; i < _dictionaryKeys.Count; ++i) {
                this.Add(_dictionaryKeys[i], _dictionaryValues[i]);
            }
        }

    }
}