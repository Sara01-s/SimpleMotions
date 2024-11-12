using System;
using System.Collections.Generic;

public interface IServices {

	void RegisterService<TInterface, TImplementation>() where TImplementation : TInterface;
	void RegisterInstance<TInterface>(TInterface instance);
	TInterface GetService<TInterface>();
	
}

public sealed class Services : IServices {
	private readonly Dictionary<Type, Type> _serviceTypes = new();
	private readonly Dictionary<Type, object> _serviceInstances = new();

	public void RegisterService<TInterface, TImplementation>() where TImplementation : TInterface {
		_serviceTypes[typeof(TInterface)] = typeof(TImplementation);
	}

	public void RegisterInstance<TInterface>(TInterface instance) {
		_serviceInstances[typeof(TInterface)] = instance;
	}

	public TInterface GetService<TInterface>() {
		return (TInterface)GetService(typeof(TInterface));
	}

	public object GetService(Type serviceType) {
		if (_serviceInstances.TryGetValue(serviceType, out var instance)) {
			return instance;
		}

		if (!_serviceTypes.TryGetValue(serviceType, out var implementationType)) {
			throw new Exception($"Service of type {serviceType} not registered.");
		}

		var serviceConstructor = implementationType.GetConstructors()[0];
		var constructorParameters = serviceConstructor.GetParameters();
		var parameterInstances = new List<object>();

		foreach (var parameter in constructorParameters) {
			var service = GetService(parameter.ParameterType);
			parameterInstances.Add(service);
		}

		instance = serviceConstructor.Invoke(parameterInstances.ToArray());
		_serviceInstances[serviceType] = instance;

		return instance;
	}
}
