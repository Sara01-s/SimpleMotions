using System.Collections.Generic;
using UnityEngine.Assertions;
using System;

namespace SimpleMotions {

    /// <summary> 
    /// Use this class to get and register SimpleMotion's services.
    /// </summary>
    public sealed class Services {

        public static Services Instance => _instance ??= new Services();

        private readonly Dictionary<Type, object> _services = new();
        private static Services _instance;

        private Services() { }

        public void RegisterService<T>(T service) {
            var serviceType = typeof(T);

            Assert.IsFalse(_services.ContainsKey(serviceType), $"Service {serviceType.Name} already registered");

            _services.Add(serviceType, service);
        }

        public void UnRegisterService<T>() {
            var serviceType = typeof(T);

            Assert.IsTrue(_services.ContainsKey(serviceType), $"Service {serviceType.Name} not registered");

            _services.Remove(serviceType);
        }

        public T GetService<T>() {
            var type = typeof(T);

            if (!_services.TryGetValue(type, out var service)) {
				throw new KeyNotFoundException(
                    $"Failed to get {type.Name} Service, try getting the service from Start() method. " 
                    + $"Note: if you're creating a new Service and it can't be referenced in the Awake() method, " 
                    + "change the execution order of your concrete service implementation to be prior."
                );
            }
            
            return (T) service;     // * This cast will never fail because T, is the same T as the RegisterService method
        }

    }
}