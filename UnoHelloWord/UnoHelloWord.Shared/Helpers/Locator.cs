using CommonServiceLocator;
using System;
using System.Globalization;
using System.Reflection;
using Unity;
using UnoHelloWord.Shared.Exceptions;
using UnoHelloWord.Shared.Services.Navigation;
using UnoHelloWord.Shared.ViewModels;
using Xamarin.Forms;

namespace UnoHelloWord.Shared.Helpers
{
    public static class Locator
    {
        private static UnityContainer _container;

        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(Locator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(Locator.AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(Locator.AutoWireViewModelProperty, value);
        }

        public static bool UseMockService { get; set; }

        static Locator()
        {
            _container = new UnityContainer();

            // View models - by default, TinyIoC will register concrete classes as multi-instance.
            _container.RegisterType<LoginViewModel>()

                ;

            // Services - by default, TinyIoC will register interface registrations as singletons.

            _container.RegisterType<INavigationService, NavigationService>()
                //.RegisterType<IIdentityService, IdentityService>()

                ;

            //TODO: Verificar a necessidade destes
            //var unityServiceLocator = new ServiceLocator();
            //ServiceLocator.SetLocatorProvider((() => unityServiceLocator));
        }

        public static void UpdateDependencies(bool useMockServices)
        {
            // Change injected dependencies
            if (useMockServices)
            {
                UseMockService = true;
            }
            else
            {
                //_container.Register<ICatalogService, CatalogService>();

                UseMockService = false;
            }
        }

        public static void RegisterSingleton<TInterface, T>() where TInterface : class where T : class, TInterface
        {
            _container.RegisterSingleton<TInterface, T>();
        }

        public static T Resolve<T>() where T : class
        {
            try
            {
                return _container.Resolve<T>();
            }
            catch (CustomException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Element;
            if (view == null)
            {
                return;
            }

            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Page.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return;
            }
            var viewModel = _container.Resolve(viewModelType);
            view.BindingContext = viewModel;
        }
    }
}