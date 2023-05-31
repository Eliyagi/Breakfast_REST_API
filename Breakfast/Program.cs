var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();

    //within a lifatime of a single request to IBreafastService use BreakfastService object,
    // and use it for all until the request is finishing
    builder.Services.AddSingleton<IBreafastService, BreakfastService>();
    
}


var app = builder.Build();
{
    // on error, send the user to this route:
    app.UseExceptionHandler("/error");

    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();

}
