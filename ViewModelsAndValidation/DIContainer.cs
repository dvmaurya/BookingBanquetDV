using System.Diagnostics.CodeAnalysis;
using Unity;
using Unity.Lifetime;
using Unity.Registration;
using Unity.Resolution;

namespace ViewModelsAndValidation
{
    public static class DIContainer
    {
        private static readonly IUnityContainer _unityContainer = new UnityContainer();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void RegisterType<TFrom, TTo>(params InjectionMember[] injectionMembers) where TTo : class, TFrom
        {
            _unityContainer.RegisterType<TFrom, TTo>(injectionMembers);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public static void RegisterSingletonType<TFrom, TTo>(params InjectionMember[] injectionMembers) where TTo : class, TFrom
        {
            _unityContainer.RegisterType<TFrom, TTo>(
                            lifetimeManager: new ContainerControlledLifetimeManager(),
                            injectionMembers: injectionMembers);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void RegisterNamedInstance<TFrom, TTo>(string name, params InjectionMember[] injectionMembers) where TTo : class, TFrom
        {
            _unityContainer.RegisterType<TFrom, TTo>(name, injectionMembers);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public static void RegisterNamedInstanceForSingletonType<TFrom, TTo>(string name, params InjectionMember[] injectionMembers) where TTo : class, TFrom
        {

            _unityContainer.RegisterType<TFrom, TTo>(
                name: name,
                lifetimeManager: new ContainerControlledLifetimeManager(),
                injectionMembers: injectionMembers);
        }

        public static void RegisterInstance<T>(T instance)
        {
            _unityContainer.RegisterInstance<T>(instance);
        }

        public static void RegisterInstanceWithName<T>(string name, T instance)
        {
            _unityContainer.RegisterInstance<T>(name, instance);
        }

        public static T GetInstance<T>(params ResolverOverride[] overrides)
        {
            return _unityContainer.Resolve<T>(overrides);
        }

        public static T GetNamedInstance<T>(string name, params ResolverOverride[] overrides)
        {
            return _unityContainer.Resolve<T>(name, overrides);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static bool IsRegistered<T>()
        {
            return _unityContainer.IsRegistered<T>();
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static bool IsRegistered<T>(string name)
        {
            return _unityContainer.IsRegistered<T>(name);
        }
    }
}
