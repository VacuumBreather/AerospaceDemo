using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using Aerospace.ViewModels;
using Autofac;
using Caliburn.Micro;
using IContainer = Autofac.IContainer;

namespace Aerospace;

internal sealed class Bootstrapper : BootstrapperBase
{
    #region Constructors and Destructors

    public Bootstrapper()
    {
        Initialize();
    }

    #endregion

    #region Protected Properties

    private IContainer? Container { get; set; }

    #endregion

    #region Protected Methods

    protected override void BuildUp(object instance) => Container!.InjectProperties(instance);

    protected override void Configure()
    {
        var builder = new ContainerBuilder();

        // Register view models
        builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
            .Where(type => type.Name.EndsWith("ViewModel"))
            .Where(type => !string.IsNullOrWhiteSpace(type.Namespace) && type.Namespace.EndsWith("ViewModels"))
            .Where(type => type.GetInterface(nameof(INotifyPropertyChanged)) != null)
            .AsSelf()
            .AsImplementedInterfaces()
            .InstancePerDependency();

        // Register views
        builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
            .Where(type => type.Name.EndsWith("View"))
            .Where(type => !string.IsNullOrWhiteSpace(type.Namespace) && type.Namespace.EndsWith("Views"))
            .AsSelf()
            .InstancePerDependency();

        // Register the single window manager for this container
        builder.Register<IWindowManager>(_ => new WindowManager()).InstancePerLifetimeScope();

        // Register the single event aggregator for this container
        builder.Register<IEventAggregator>(_ => new EventAggregator()).InstancePerLifetimeScope();

        Container = builder.Build();
    }

    protected override IEnumerable<object>? GetAllInstances(Type service) =>
        Container!.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;

    protected override object GetInstance(Type service, string? key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            if (Container!.IsRegistered(service))
            {
                return Container!.Resolve(service);
            }
        }
        else
        {
            if (Container!.IsRegisteredWithKey(key, service))
            {
                return Container!.ResolveKeyed(key, service);
            }
        }

        throw new Exception($"Could not locate any instances of contract {key ?? service.Name}.");
    }

    protected override async void OnStartup(object sender, StartupEventArgs e) =>
        await DisplayRootViewForAsync<MainViewModel>();

    protected override IEnumerable<Assembly> SelectAssemblies() => new[] {GetType().Assembly};

    #endregion
}