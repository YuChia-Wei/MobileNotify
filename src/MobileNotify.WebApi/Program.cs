using dotAPNS.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using MobileNotify.WebApi.Certificates;
using MobileNotify.WebApi.Extensions;
using MobileNotify.WebApi.Options;
using MobileNotify.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.AddFirebaseServices();

builder.Services.AddApns();
builder.Services.Configure<ApnsSettingOptions>(builder.Configuration.GetSection(ApnsSettingOptions.SectionName));

builder.Services.Configure<AzureKeyVaultSettingOptions>(builder.Configuration.GetSection(AzureKeyVaultSettingOptions.SectionName));
builder.Services.AddScoped<IAzureKeyVaultClientFactory, AzureKeyVaultClientFactory>();
builder.Services.AddScoped<ICertificateProvider, AzureKeyVaultCertificateProvider>();
// builder.Services.AddScoped<ICertificateProvider, LocalFileCertificateProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapPost("apns/voip/{deviceToken}", async (
                [FromRoute] string deviceToken,
                [FromServices] ApplePushNotificationService service) =>
            {
                var result = await service.SendVoipAsync(deviceToken);
                return Results.Ok(result);
            });

app.MapPost("fcm/{deviceToken}", async (
                [FromRoute] string deviceToken,
                [FromServices] FirebaseCloudMessagingService firebaseCloudMessagingService) =>
            {
                var result = await firebaseCloudMessagingService.PushAsync(deviceToken);
                return Results.Ok(result);
            });

app.Run();