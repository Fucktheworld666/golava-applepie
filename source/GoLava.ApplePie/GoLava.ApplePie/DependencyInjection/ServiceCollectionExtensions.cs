using System;
using System.Linq;
using GoLava.ApplePie.Clients.AppleDeveloper;
using GoLava.ApplePie.Clients.ApplePie;
using GoLava.ApplePie.Serializers;
using GoLava.ApplePie.Transfer;
using GoLava.ApplePie.Transfer.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace GoLava.ApplePie.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplePie(this IServiceCollection services, Action<ApplePieOptions> setup = null)
        {
            if (services.Any(x => x.ServiceType == typeof(ApplePieOptions)))
                throw new InvalidOperationException("ApplePie services already registered.");

            var options = new ApplePieOptions();
            setup?.Invoke(options);

            services.AddSingleton<IAppleDeveloperUrlProvider>(options.AppleDeveloperFactory);
            services.AddSingleton<ICertificateStore>(options.CertificateStoreFactory);
            services.AddSingleton<ApplePieOptions>(options);

            var jsonSerializer = new Serializers.JsonSerializer(new JsonSerializerSettings
            {
                ContractResolver = new CustomPropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });

            services.AddSingleton<IJsonSerializer>(jsonSerializer);
            services.AddSingleton<IRestClient, RestClient>();

            services.AddSingleton<AppleDeveloperClient>();
            services.AddSingleton<ApplePieClient>();
        }
    }
}
