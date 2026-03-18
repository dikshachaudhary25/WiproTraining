var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseExceptionHandler("/error");
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.Use(async (context, next) =>
{
    context.Response.Headers["Content-Security-Policy"] =
        "default-src 'self'; script-src 'self'; style-src 'self';";

    await next();
});
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");

    await next();

    Console.WriteLine($"Response Status Code: {context.Response.StatusCode}");
});
app.Map("/error", errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync("<h1>Something went wrong!</h1><p>Please try again later.</p>");
    });
});
app.MapGet("/", () => "Middleware Demo Running...");
app.MapGet("/test-error", () =>
{
    throw new Exception("Test exception");
});

app.Run();
