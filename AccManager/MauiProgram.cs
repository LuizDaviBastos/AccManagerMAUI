using AccManager.Data;

namespace AccManager
{
    using AccManager.Data.Models;
    using AccManager.Data.Models.ModelSettings;
    using AccManager.Data.MongoServicoGenerico;
    using System.Reflection;

    public static class MauiProgram
    {

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddBlazorWebView();
            builder.Services.AddSingleton<WeatherForecastService>();

            var a = Assembly.GetExecutingAssembly();
            var stream = a.GetManifestResourceStream("AccManager.appsettings.json");
            stream.Position = 0;

            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();

            var jobj = JObject.Parse(json);
            var mongoSettings = jobj.GetValue("EnvioDeContasMongoSettings");
            var smtpSettings = jobj.GetValue("ConfiguracaoSmtp");

            var mongoValues = mongoSettings.ToObject<EnvioDeContasMongoSettings>();

            builder.Services.AddSingleton<IEnvioDeContasMongoSettings>(x => mongoValues);

            builder.Services.AddScoped<IMongoServico<HistoricoEnvio>, MongoServico<HistoricoEnvio>>();
            builder.Services.AddScoped<IMongoServico<Contas>, MongoServico<Contas>>();
            builder.Services.AddScoped<IMongoServico<Plataforma>, MongoServico<Plataforma>>();
            builder.Services.AddScoped<IMongoServico<Arquivo>, MongoServico<Arquivo>>();
            builder.Services.AddScoped<IMongoServico<Layout>, MongoServico<Layout>>();
            builder.Services.AddScoped<MongoClient>(sp => new MongoClient(mongoValues.ConnectionString));

            builder.Services.AddScoped<IMongoServico<Contas>, MongoServico<Contas>>();

            return builder.Build();
        }
    }
}


