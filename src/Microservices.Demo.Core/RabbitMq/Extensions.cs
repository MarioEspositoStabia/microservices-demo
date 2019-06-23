using Microservices.Demo.Core.Commands;
using Microservices.Demo.Core.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Instantiation;
using RawRabbit.Operations.Subscribe.Context;
using System.Reflection;

namespace Microservices.Demo.Core.RabbitMq
{
    public static class Extensions
    {
        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            RabbitMqOptions rabbitMqOptions = new RabbitMqOptions();
            IConfigurationSection configurationSection = configuration.GetSection("rabbitmq");
            configurationSection.Bind(rabbitMqOptions);

            RawRabbit.Instantiation.Disposable.BusClient busClient = RawRabbitFactory.CreateSingleton(new RawRabbitOptions { ClientConfiguration = rabbitMqOptions });
            services.AddSingleton<IBusClient>(serviceProvider => busClient);
        }

        public static void RabbitMqSubscribeToCommand<TCommand>(this IApplicationBuilder applicationBuilder) where TCommand : ICommand
        {
            string queueName = GetTypeName<TCommand>();

            using (IServiceScope serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                IBusClient busClient = serviceScope.ServiceProvider.GetService<IBusClient>();
                ICommandHandler<TCommand> commandHandler = serviceScope.ServiceProvider.GetService<ICommandHandler<TCommand>>();

                busClient.SubscribeAsync<TCommand>(command => commandHandler.HandleAsync(command), 
                                                   subscribeContext => subscribeContext.SubscribeWithName(queueName));
            }

        }

        public static void RabbitMqSubscribeToEvent<TEvent>(this IApplicationBuilder applicationBuilder) where TEvent : IEvent
        {
            string queueName = GetTypeName<TEvent>();

            using (IServiceScope serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                IBusClient busClient = serviceScope.ServiceProvider.GetService<IBusClient>();
                IEventHandler<TEvent> eventHandler = serviceScope.ServiceProvider.GetService<IEventHandler<TEvent>>();

                busClient.SubscribeAsync<TEvent>(@event => eventHandler.HandleAsync(@event),
                                                 subscribeContext => subscribeContext.SubscribeWithName(queueName));
            }

        }

        private static string GetTypeName<T>()
        {
            return $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}";
        }

        private static void SubscribeWithName(this ISubscribeContext subscribeContext, string queueName)
        {
            subscribeContext.UseSubscribeConfiguration(configuration => configuration.FromDeclaredQueue(queue => queue.WithName(queueName)));
        }
    }
}
