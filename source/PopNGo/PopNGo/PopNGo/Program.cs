using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PopNGo.Areas.Identity.Data;
using PopNGo.Data;
using PopNGo.Models;
using PopNGo.Services;
using Microsoft.OpenApi.Models;
using PopNGo.DAL.Abstract;
using PopNGo.DAL.Concrete;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication;

namespace PopNGo;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Setup the connection string to our Identity database
        // Swap the commented out lines to switch between Local and Azure databases
        var identityConnectionString = builder.Configuration.GetConnectionString("IdentityConnection");
        // var identityConnectionString = builder.Configuration.GetConnectionString("IdentityConnectionAzure");
        builder.Services.AddDbContext<ApplicationDbContext>(options => options
            .UseSqlServer(identityConnectionString)
            .UseLazyLoadingProxies()
        );
        
        // Setup the connection string to our Application database
        // Swap the commented out lines to switch between Local and Azure databases
        var serverConnectionString = builder.Configuration.GetConnectionString("ServerConnection");
        // var serverConnectionString = builder.Configuration.GetConnectionString("ServerConnectionAzure");
        builder.Services.AddDbContext<PopNGoDB>(options => options
            .UseSqlServer(serverConnectionString)
            .UseLazyLoadingProxies()
        );
      
        // Setup all of our REST API services
        //REST API setup for Real Time Event Search API
        string realTimeEventSearchApiKey = builder.Configuration["RealTimeEventSearchApiKey"];
        string realTimeEventSearchUrl = "https://real-time-events-search.p.rapidapi.com/";

        builder.Services.AddHttpClient<IRealTimeEventSearchService, RealTimeEventSearchService>((httpClient, services) =>
        {
            httpClient.BaseAddress = new Uri(realTimeEventSearchUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", realTimeEventSearchApiKey); // Set API key
            httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", "real-time-events-search.p.rapidapi.com");
            return new RealTimeEventSearchService(httpClient, services.GetRequiredService<ILogger<RealTimeEventSearchService>>());
        });

        // REST API setup for the Distance Calculator API
        string distanceCalculatorApiKey = builder.Configuration["DistanceAndWeatherRapidAPIKey"];
        string distanceCalculatorUrl = "https://distance-calculator.p.rapidapi.com/v1/";

        builder.Services.AddHttpClient<IDistanceCalculatorService, DistanceCalculatorService>((httpClient, services) =>
        {
            httpClient.BaseAddress = new Uri(distanceCalculatorUrl);           
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", distanceCalculatorApiKey); // Set API key
            httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", "distance-calculator.p.rapidapi.com");
            return new DistanceCalculatorService(httpClient, services.GetRequiredService<ILogger<DistanceCalculatorService>>());
        });

        // REST API setup for the Weather Forecast API
        string weatherForecasterUrl = "https://visual-crossing-weather.p.rapidapi.com/forecast";

        builder.Services.AddHttpClient<IWeatherForecastService, WeatherForecastService>((httpClient, services) =>
        {
            httpClient.BaseAddress = new Uri(weatherForecasterUrl);
            httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", distanceCalculatorApiKey); // Set API key
            httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", "visual-crossing-weather.p.rapidapi.com");
            return new WeatherForecastService(httpClient, services.GetRequiredService<ILogger<WeatherForecastService>>());
        });

        // REST API setup for the Place Suggestions API
        string placeSuggestionsUrl = "https://serpapi.com/search.json?";
        string placeSuggestionsApiKey = builder.Configuration["SerpMapApiKey"];

        builder.Services.AddHttpClient<IPlaceSuggestionsService, PlaceSuggestionsService>((httpClient, services) =>
        {
            httpClient.BaseAddress = new Uri(placeSuggestionsUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json"); // Accept JSON responses
            httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", placeSuggestionsApiKey); // Set API key in Authorization header if needed
            return new PlaceSuggestionsService(httpClient, services.GetRequiredService<ILogger<PlaceSuggestionsService>>());
        });

        // Add services to the container.
        builder.Services.AddScoped<DbContext,PopNGoDB>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IEventHistoryRepository, EventHistoryRepository>();
        builder.Services.AddScoped<IPgUserRepository, PgUserRepository>();
        builder.Services.AddScoped<ITagRepository, TagRepository>();
        builder.Services.AddScoped<IFavoritesRepository, FavoritesRepository>();
        builder.Services.AddScoped<IEventRepository, EventRepository>();
        builder.Services.AddScoped<IBookmarkListRepository, BookmarkListRepository>();
        builder.Services.AddScoped<IScheduledNotificationRepository, ScheduledNotificationRepository>();
        builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
        builder.Services.AddScoped<ISearchRecordRepository, SearchRecordRepository>();
        builder.Services.AddScoped<IEmailHistoryRepository, EmailHistoryRepository>();
        builder.Services.AddScoped<IAccountRecordRepository, AccountRecordRepository>();
        builder.Services.AddScoped<IItineraryEventRepository, ItineraryEventRepository>();
        builder.Services.AddScoped<IItineraryRepository, ItineraryRepository>();
        builder.Services.AddScoped<IEventTagRepository, EventTagRepository>();

        // Add Google Authentication
        builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["GoogleAuthenticationSecretId"];
                googleOptions.ClientSecret = builder.Configuration["GoogleAuthenticationSecretKey"];

                googleOptions.SaveTokens = true;

                googleOptions.Events.OnCreatingTicket = ctx =>
                {
                    List<AuthenticationToken> tokens = ctx.Properties.GetTokens().ToList();

                    tokens.Add(new AuthenticationToken()
                    {
                        Name = "TicketCreated",
                        Value = DateTime.UtcNow.ToString()
                    });

                    ctx.Properties.StoreTokens(tokens);

                    return Task.CompletedTask;
                };
            }
        );

        // Add Identity
        builder.Services.AddDefaultIdentity<PopNGoUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Tokens.ProviderMap.Add("CustomEmailConfirmation",
                    new TokenProviderDescriptor(
                    typeof(CustomEmailConfirmationTokenProvider<PopNGoUser>)));
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
            }
        )
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();
        
        // Add Email-related services
        builder.Services.AddTransient<CustomEmailConfirmationTokenProvider<PopNGoUser>>();
        builder.Services.AddTransient<EmailSender>();
        builder.Services.AddTransient<EmailBuilder>();
        builder.Services.AddHostedService<TimedEmailService>();

        // Add the services for Controllers
        builder.Services.AddControllersWithViews();

        // Add Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
        });

        // Configure Forwarded Headers
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        var app = builder.Build();

        // Link the services to the ScheduleTasking class
        ScheduleTasking.SetServiceScopeFactory(app.Services.GetRequiredService<IServiceScopeFactory>());

        // Seed the database with the Admin user and roles
        SeedData(app).Wait();

        // Configure environment settings
        if (app.Environment.IsDevelopment())
        {
            // app.UseMigrationsEndPoint();
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
            });
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseForwardedHeaders();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }

    // Seed the database with the Admin user and roles
    // This method is called by the Main method on startup
    // It checks if the Admin user exists and creates it if it does not
    // Then it checks if the Admin and User roles exists and creates them if they do not
    // Finally, it checks if the Admin user is in the Admin role and adds it if it is not
    public static async Task SeedData(WebApplication app) {
        using (var scope = app.Services.CreateScope()) {
            PopNGoDB _popNGoDBContext = scope.ServiceProvider.GetRequiredService<PopNGoDB>();
            UserManager<PopNGoUser> userSeeder = scope.ServiceProvider.GetRequiredService<UserManager<PopNGoUser>>();
            RoleManager<IdentityRole> roleSeeder = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var userExists = await userSeeder.FindByNameAsync("admin@popngo.com");
            if (userExists == null) {
                var user = Activator.CreateInstance<PopNGoUser>();
                user.UserName = "admin@popngo.com";
                user.Email = "popngo.wou@gmail.com";
                user.EmailConfirmed = true;
                user.FirstName = "The";
                user.LastName = "Admin";

                await userSeeder.CreateAsync(user, configuration["AdminPW"]);
                PgUser newUser = new()
                {
                    AspnetuserId = user.Id
                };
                _popNGoDBContext.PgUsers.Add(newUser);
                await _popNGoDBContext.SaveChangesAsync();
            }

            var roleExists = await roleSeeder.RoleExistsAsync("Admin");
            if (!roleExists) {
                await roleSeeder.CreateAsync(new IdentityRole("Admin"));
            }
            var roleUserExists = await roleSeeder.RoleExistsAsync("User");
            if (!roleUserExists) {
                await roleSeeder.CreateAsync(new IdentityRole("User"));
            }

            var adminUser = await userSeeder.FindByNameAsync("admin@popngo.com");
            if (!await userSeeder.IsInRoleAsync(adminUser, "Admin")) {
                await userSeeder.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
