using Autofac;

namespace QuantumChess.App.Framework
{
	public class WpfFrameworkModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<WindowManager>().AsImplementedInterfaces().SingleInstance();
			builder.RegisterType<DialogManager>().AsImplementedInterfaces().SingleInstance();
		}
	}
}
