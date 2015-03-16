﻿using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Navigation;
using Prism.Presentation;
using Prism.Services;

namespace Prism.Unity
{
    public abstract class UnityBootstrapper : Bootstrapper
    {
        public IUnityContainer Container { get; protected set; }

        public override void Run()
        {
            Container = CreateContainer();
            NavigationService = CreateNavigationService();

            ConfigureContainer();
            ConfigureServiceLocator();
            RegisterTypes();

            App.MainPage = CreateMainPage();
            InitializeMainPage();
        }

        protected override void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory((type) => Container.Resolve(type));
        }

        protected virtual void InitializeMainPage()
        {
        }

        protected virtual IUnityContainer CreateContainer()
        {
            return new UnityContainer();
        }

        protected virtual INavigationService CreateNavigationService()
        {
            return new PageNavigationService();
        }

        protected virtual void ConfigureContainer()
        {
            Container.RegisterInstance<INavigationService>(NavigationService);
            Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IServiceLocator, UnityServiceLocatorAdapter>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IDependencyService, DependencyService>(new ContainerControlledLifetimeManager());
        }

        protected override void ConfigureServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => this.Container.Resolve<IServiceLocator>());
        }
    }
}
