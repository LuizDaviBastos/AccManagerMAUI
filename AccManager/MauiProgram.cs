using Microsoft.AspNetCore.Components.WebView.Maui;
using AccManager.Data;
using AccManager.Data.MongoServicoGenerico;
using AccManager.Data.Models;
using MongoDB.Driver;
using AccManager.Data.Models.ModelSettings;

namespace AccManager 
{
	using AccManager.Data.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Options;
    using System.Reflection;

    public static class MauiProgram
	{
		private static IConfiguration Configuration { get; set; }

		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.RegisterBlazorMauiWebView()
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				})/*
			.Host
			.ConfigureAppConfiguration((app, config) =>
			{
				var assembly = typeof(App).GetTypeInfo().Assembly;
				var configuration = config.AddJsonFile(new EmbeddedFileProvider(assembly), "appsettings.json", optional: false, false);
				Configuration = configuration.Build();
			})*/;

			builder.Services.AddBlazorWebView();
			builder.Services.AddSingleton<WeatherForecastService>();

            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("AccManager.appsettings.json");

            Configuration = new ConfigurationBuilder().AddJsonStream(stream).Build();
            builder.Configuration.AddConfiguration(Configuration);

			builder.Services.Configure<EnvioDeContasMongoSettings>(Configuration.GetSection(nameof(EnvioDeContasMongoSettings)));
			builder.Services.AddSingleton<IEnvioDeContasMongoSettings>(sp => sp.GetRequiredService<IOptions<EnvioDeContasMongoSettings>>().Value);

			builder.Services.AddScoped<IMongoServico<HistoricoEnvio>, MongoServico<HistoricoEnvio>>();
			builder.Services.AddScoped<IMongoServico<Contas>, MongoServico<Contas>>();
			builder.Services.AddScoped<IMongoServico<Plataforma>, MongoServico<Plataforma>>();
			builder.Services.AddScoped<IMongoServico<Arquivo>, MongoServico<Arquivo>>();
			builder.Services.AddScoped<IMongoServico<Layout>, MongoServico<Layout>>();
			builder.Services.AddScoped<MongoClient>(sp => new MongoClient(Configuration.GetSection(nameof(EnvioDeContasMongoSettings))["ConnectionString"]));

			builder.Services.AddScoped<IMongoServico<Contas>, MongoServico<Contas>>();

			return builder.Build();
		}
	}
}


