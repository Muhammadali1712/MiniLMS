using MiniLMS.Application;
using MiniLMS.Infrastructure;
using Serilog;
using Serilog.Core;
using Serilog.Formatting.Display;

namespace MiniLMS;
public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        var botToken = "1014298353:AAHwmo0n1pnK7zk-zf2bomdiqTZnAEee4Gk";

        Logger log = new LoggerConfiguration()
           .Enrich.WithEnvironmentUserName()
           .Enrich.WithThreadId()
           .Enrich.WithEnvironmentName()
           .Enrich.WithThreadName()
           .Enrich.WithMachineName()
           .WriteTo.Console(outputTemplate: "[ ThreadId: {ThreadId} {ThreadName} {MachineName} {EnvironmentUserName} {Level:u3}] {Message:1j}{NewLine}{Exception}")
                   .WriteTo.File("my_log.txt")
                   .WriteTo.File("My_json_log.json")
                   .WriteTo.Telegram(botToken: botToken, chatId: "971153825")
                   .WriteTo.PostgreSQL("Host=::1; Database=serilog;User Id=postgres; password=2004-12-17;", "serilog1",needAutoCreateTable : true)
                   .CreateLogger();
        log.Information("salom");
        log.Fatal("Fatal");



        // CreateAsync services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddApplicationServise();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        //builder.Services.AddLogging();
        builder.Services.AddSerilog(log);

        builder.Services.AddStackExchangeRedisCache(setupAction =>
        {
            setupAction.Configuration = "127.0.0.1:6379";
        });

        builder.Services.AddLazyCache();

       builder.Services.AddMediatR(mdr=>mdr.RegisterServicesFromAssemblies(typeof(Program).Assembly));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c=>c.DisplayRequestDuration());
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
