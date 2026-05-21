using System;
using System.Collections.Generic;
using UnityEngine;

namespace PortfolioFilling.Core
{
    public sealed class GameRegistry : MonoBehaviour
    {
        private static GameRegistry _instance;
        private readonly Dictionary<Type, object> _services = new();

        public static GameRegistry Instance => _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        public void Register<T>(T service) where T : class
        {
            if (service == null)
            {
                return;
            }

            _services[typeof(T)] = service;
        }

        public T Get<T>() where T : class
        {
            return TryGet<T>(out var service) ? service : null;
        }

        public bool TryGet<T>(out T service) where T : class
        {
            if (_services.TryGetValue(typeof(T), out var raw))
            {
                service = raw as T;
                return service != null;
            }

            service = null;
            return false;
        }
    }
}
