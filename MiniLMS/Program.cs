using Microsoft.AspNetCore.RateLimiting;
using MiniLMS.Application;
using MiniLMS.Domain.Models;
using MiniLMS.Infrastructure;
using System.Threading.RateLimiting;

namespace MiniLMS;
public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);


        var myOptions = new MyRateLimitOptions();
        builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);



        // CreateAsync services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddApplicationServise();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddLogging();


        builder.Services.AddRateLimiter(_ => _.AddFixedWindowLimiter("fixed", options =>
        {
            options.PermitLimit = 3;
            options.Window = TimeSpan.FromSeconds(20);
            //options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //options.QueueLimit = 2;
        }));

        builder.Services.AddRateLimiter(_ => _
    .AddTokenBucketLimiter(policyName: "token", options =>
    {
        options.TokenLimit = 4;
       // options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
       // options.QueueLimit = 2;
        options.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
        options.TokensPerPeriod = 3;
    }));


        builder.Services.AddRateLimiter(_ => _
    .AddSlidingWindowLimiter(policyName: "sliding", options =>
    {
        options.PermitLimit = myOptions.PermitLimit;
        options.Window = TimeSpan.FromSeconds(myOptions.Window);
        options.SegmentsPerWindow = myOptions.SegmentsPerWindow;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = myOptions.QueueLimit;
    }));


        builder.Services.AddRateLimiter(_ => _
    .AddConcurrencyLimiter(policyName: "concurrency", options =>
    {
        options.PermitLimit = myOptions.PermitLimit;
        
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = myOptions.QueueLimit;
    }));

        builder.Services.AddMediatR(mdr => mdr.RegisterServicesFromAssemblies(typeof(Program).Assembly));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.DisplayRequestDuration());
        }

        app.UseRateLimiter();

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
