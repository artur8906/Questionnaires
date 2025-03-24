using Core.Interfaces;
using Infrastructure.Services;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<QuestionFileSettings>(
            builder.Configuration.GetSection("QuestionFileSettings"));

        builder.Services.AddSingleton<IQuestionService>(provider =>
        {
            var env = provider.GetRequiredService<IWebHostEnvironment>();
            var settings = provider.GetRequiredService<IOptions<QuestionFileSettings>>().Value;

            var questionsPath = Path.Combine(env.ContentRootPath, settings.QuestionsFilePath);
            var responsesPath = Path.Combine(env.ContentRootPath, settings.ResponsesFilePath);

            return new FileQuestionService(questionsPath, responsesPath);
        });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
