using Autofac;

namespace QuantumChess.App.Framework
{
	public class ViewModelFrameworkModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<EventAggregator>().AsImplementedInterfaces().SingleInstance();
		}
	}
}
