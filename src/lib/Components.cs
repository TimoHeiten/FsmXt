using System;
using System.Collections.Generic;
using System.Linq;
using heitech.FsmXt;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Components
    {
        ///<summary>
        /// Register a specific StateMachine as Singleton service.
        ///</summary>
        public static IServiceCollection AddStateMachine(this IServiceCollection services, Action errorCallback)
        {
            var registers = AllStates(services);
            var machine = new StateMachine(registers, errorCallback);
            services.AddSingleton<IStateMachine>(machine);

            return services;
        }
        
        private static IEnumerable<StateRegister> AllStates(IServiceCollection services)
        {
            services.Scan
            (
                scan => scan.FromEntryAssembly()
                            .AddClasses(classes => classes.AssignableTo(typeof(IState<>)))
                            .AsSelfWithInterfaces()
                            .WithSingletonLifetime()
            );
            var scope = services.BuildServiceProvider().CreateScope();
            var states = scope.ServiceProvider.GetServices<IState>();

            return states.Select(state => new StateRegister(state.Name, state));
        }
    }
}