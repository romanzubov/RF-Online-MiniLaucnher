using System;
using System.Collections.Generic;

namespace MiniLauncherStyle.Core
{
    /// <summary>
    /// Простой Service Locator для Dependency Injection.
    /// </summary>
    public class ServiceLocator
    {
        private static ServiceLocator instance;
        private static readonly object syncLock = new object();

        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();
        private readonly Dictionary<Type, Func<object>> factories = new Dictionary<Type, Func<object>>();

        private ServiceLocator()
        {
        }

        /// <summary>
        /// Singleton экземпляр ServiceLocator.
        /// </summary>
        public static ServiceLocator Current
        {
            get
            {
                if (instance == null)
                {
                    lock (syncLock)
                    {
                        if (instance == null)
                        {
                            instance = new ServiceLocator();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Регистрирует singleton экземпляр сервиса.
        /// </summary>
        /// <typeparam name="TInterface">Тип интерфейса</typeparam>
        /// <param name="implementation">Экземпляр реализации</param>
        public void Register<TInterface>(TInterface implementation) where TInterface : class
        {
            Type type = typeof(TInterface);
            lock (services)
            {
                services[type] = implementation;
            }
        }

        /// <summary>
        /// Регистрирует фабрику для создания сервиса.
        /// </summary>
        /// <typeparam name="TInterface">Тип интерфейса</typeparam>
        /// <param name="factory">Фабрика создания экземпляра</param>
        public void RegisterFactory<TInterface>(Func<TInterface> factory) where TInterface : class
        {
            Type type = typeof(TInterface);
            lock (factories)
            {
                factories[type] = () => factory();
            }
        }

        /// <summary>
        /// Получает зарегистрированный сервис.
        /// </summary>
        /// <typeparam name="TInterface">Тип интерфейса</typeparam>
        /// <returns>Экземпляр сервиса</returns>
        public TInterface Get<TInterface>() where TInterface : class
        {
            Type type = typeof(TInterface);

            lock (services)
            {
                if (services.ContainsKey(type))
                {
                    return (TInterface)services[type];
                }
            }

            lock (factories)
            {
                if (factories.ContainsKey(type))
                {
                    return (TInterface)factories[type]();
                }
            }

            throw new InvalidOperationException(
                string.Format("Service of type {0} is not registered.", type.Name));
        }

        /// <summary>
        /// Проверяет, зарегистрирован ли сервис.
        /// </summary>
        /// <typeparam name="TInterface">Тип интерфейса</typeparam>
        public bool IsRegistered<TInterface>() where TInterface : class
        {
            Type type = typeof(TInterface);
            
            lock (services)
            {
                if (services.ContainsKey(type))
                {
                    return true;
                }
            }

            lock (factories)
            {
                if (factories.ContainsKey(type))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Сбрасывает все регистрации (для тестов).
        /// </summary>
        public void Reset()
        {
            lock (services)
            {
                services.Clear();
            }
            lock (factories)
            {
                factories.Clear();
            }
        }
    }
}
