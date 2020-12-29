using Autofac;
using QuantumChess.App.Framework;

namespace QuantumChess.App
{
	public class AppModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterAssemblyTypes(ThisAssembly)
			       .AssignableTo<ViewModelBase>()
			       .AsImplementedInterfaces()
			       .AsSelf();
		}
	}
}