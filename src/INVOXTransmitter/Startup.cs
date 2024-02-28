using INVOXTransmitter.Application.ServiceEngine;
using INVOXTransmitter.Business.Policies;
using INVOXTransmitter.Business.Repositories;
using INVOXTransmitter.Business.Transmission;
using INVOXTransmitter.Infraestructure.Transmission;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

[assembly: FunctionsStartup(typeof(INVOXTransmitter.Startup))]

namespace INVOXTransmitter
{
    [ExcludeFromCodeCoverage]
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<ITransmitterEngine, VoiceFilesTransmitterEngine>();
            builder.Services.AddTransient<IPoliciesValidator, PoliciesValidator>();
            builder.Services.AddTransient<IFileRepository, Infraestructure.Repositories.FileRepository>(di =>
            {
                var connectionString = di.GetRequiredService<IConfiguration>().GetSection("AzureWebJobsStorage").Value;
                return new Infraestructure.Repositories.FileRepository(connectionString);
            });

            builder.Services.AddHttpClient("transcriptClient", (sp,c) =>
                c.BaseAddress = new Uri(sp.GetRequiredService<IConfiguration>().GetSection("TranscriptBaseAddress").Value)
            );
            builder.Services.AddTransient<ITransmitter, Transmitter>(sp =>
                {                                      
                    return new Transmitter(sp.GetService<IHttpClientFactory>().CreateClient("transcriptClient"), sp.GetService<ILoggerFactory>().CreateLogger<Transmitter>());
                }
            );
        }
    }
}
