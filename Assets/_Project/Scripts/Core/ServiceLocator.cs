using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatFootball.Core
{
    /// <summary>
    /// Service Locator pattern for managing game services
    /// Provides loose coupling and dependency injection
    /// </summary>
    public class ServiceLocator
    {
        private static ServiceLocator _instance;
        public static ServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceLocator();
                }
                return _instance;
            }
        }

        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        /// <summary>
        /// Register a service
        /// </summary>
        public void Register<T>(T service) where T : class
        {
            Type type = typeof(T);
            if (_services.ContainsKey(type))
            {
                Debug.LogWarning($"Service of type {type.Name} is already registered. Overwriting.");
                _services[type] = service;
            }
            else
            {
                _services.Add(type, service);
                Debug.Log($"Service registered: {type.Name}");
            }
        }

        /// <summary>
        /// Unregister a service
        /// </summary>
        public void Unregister<T>() where T : class
        {
            Type type = typeof(T);
            if (_services.ContainsKey(type))
            {
                _services.Remove(type);
                Debug.Log($"Service unregistered: {type.Name}");
            }
        }

        /// <summary>
        /// Get a registered service
        /// </summary>
        public T Get<T>() where T : class
        {
            Type type = typeof(T);
            if (_services.TryGetValue(type, out object service))
            {
                return service as T;
            }

            Debug.LogError($"Service of type {type.Name} not found!");
            return null;
        }

        /// <summary>
        /// Check if service is registered
        /// </summary>
        public bool Has<T>() where T : class
        {
            return _services.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Clear all services (useful for cleanup)
        /// </summary>
        public void Clear()
        {
            _services.Clear();
            Debug.Log("All services cleared");
        }
    }
}
