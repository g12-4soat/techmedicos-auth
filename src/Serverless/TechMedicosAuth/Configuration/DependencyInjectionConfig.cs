using Amazon;
using Amazon.CognitoIdentityProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TechMedicosAuth.Service;
using AWSOptions = TechMedicosAuth.AWS.Options.AWSOptions;

namespace TechMedicosAuth.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<ICognitoService, CognitoService>();

            services.AddScoped<ICognitoService>(x =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var opt = serviceProvider.GetRequiredService<IOptions<AWSOptions>>();

                var provider = new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(opt.Value.Region));
                var client = new AmazonCognitoIdentityProviderClient();

                return new CognitoService(opt, client, provider);
            });
        }
    }
}
