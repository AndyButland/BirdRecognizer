namespace BirdRecognizer.Predict.Extensions
{
    using System;
    using BirdRecognizer.Common.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class AzureBlobStorageServiceCollectionExtensions
    {
        public static IServiceCollection AddStorageService(this IServiceCollection collection, 
                                                           IConfiguration config)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            collection.Configure<AzureBlobStorageServiceOptions>(config);
            return collection.AddTransient<IStorageService, AzureBlobStorageService>();
        }
    }
}
