# Build an API using .Net Core 3.1, Swagger, Nlog

## .NET CORE Web Application Steps
1. Create a project using Web Application with Authentication.(Not API)
2. Remove unnecessary files, folders (wwwroot, Areas and Pages folders) 
3. Make some changes to startup.cs
```
public void ConfigureServices(IServiceCollection services)
        {
            delete=> services.AddRazorPages(); //not needed for api
           
            add=> services.AddControllers();
        }

 public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            delete=> app.UseStaticFiles(); 
            app.UseEndpoints(endpoints =>
            {
                change from=> endpoints.MapRazorPages();
                to=> endpoints.MapControllers();
            });
        }
```
4. Add Swagger to your project
 
   1. Install NuGets:
     - Swashbuckle.AspNetCore.Swagger
     - Swashbuckle.AspNetCore.SwaggerUi
     - Swashbuckle.AspNetCore.SwaggerGen  
   2. Include Swagger service in Startup.cs
        ```
        public void ConfigureServices(IServiceCollection services)
        {
           services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("version1", new OpenApiInfo
                {
                    Title = "NCNews API",
                    Version = "version1",
                    Description = "An educational API for a new feed page"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/version1/swagger.json", "NCNews API");
                c.RoutePrefix = "";
            });        
        }
        ```
   3. Include xml documentation
        
        Right click on Project and go to Properties/Build => 'Output' section tick XML documentation file.
        Also 'Errors and warnings' section add warning code '1591' then Save!

        Add some extra lines to AddSwaggerGen() to create the XML file dynamically
         ```
         public void ConfigureServices(IServiceCollection services)
         {
           services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("version1", new OpenApiInfo
                {
                    Title = "NCNews API",
                    Version = "version1",
                    Description = "An educational API for a new feed page"
                });

                var xfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xpath = Path.Combine(AppContext.BaseDirectory, xfile);
                c.IncludeXmlComments(xpath);                
            });
         }
        ```
5. Add NLog to the project
    1. Install 'NLog.Extensions.Logging' from NuGet Manager:
    2. Create a new file in projects main folder 'nlog.config', set the paths for both log files
        ```
        <?xml version="1.0" encoding="utf-8" ?>
        <nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
              xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
              autoReload="true"
              internalLogLevel="Trace"
              internalLogFile="c:/****PATH****/internallog.txt"> 

          <targets>
            <target name="logfile" xsi:type="File"
                    fileName="c:/****PATH****/${shortdate}_logfile.txt"
                    layout="${longdate} ${level:uppercase=true} ${message}"/>
          </targets>

          <rules>
            <logger name="*" minlevel="Debug" writeTo="logfile" />
          </rules>
        </nlog>
        ```
    3. Add two folders to the project
      - Contracts - for Interfaces
      - Services - for Implementations 

    4. Add ILoggerService and LoggerService
        ```
        public interface ILoggerService
        {
            void LogInfo(string message);
            void LogWarn(string message);
            void LogDebug(string message);
            void LogError(string message);
        }

        public class LoggerService : ILoggerService
        {
            private static ILogger logger = LogManager.GetCurrentClassLogger();

            public void LogDebug(string message)
            {
                logger.Debug(message);
            }

            public void LogError(string message)
            {
                logger.Error(message);
            }

            public void LogInfo(string message)
            {
                logger.Info(message);
            }

            public void LogWarn(string message)
            {
                logger.Warn(message);
            }
        }
        ```
    5. Make LoggerService available(Dependency Injection) for the entire project in Startup.cs
        ```
            services.AddSingleton<ILoggerService, LoggerService>();
        ```
6. Configure CORS(Cross Origin Resource Sharing) policy
    Enables interaction across the pipe(Globally)
    
    Add Cors to ConfigureServices() (Startup.cs)
        
    ```
        services.AddCors(o =>
        {
            o.AddPolicy("CorsPolicy", builder =>
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
        });
    ```
    and to Configure()
    ```
        app.UseCors("CorsPolicy");
    ```
7. Add EF to the project
  -   Microsoft.EntityFrameworkCore.InMemory
  -   Microsoft.EntityFrameworkCore.SqlServer

8. Add AutoMapper to the project
  - Install AutoMapper
  - Install AutoMapper.Extensions.Microsoft.DependencyInjection
  - Create a folder called Mappings
  - Add new Class Maps.cs    
  - Inherit Maps from Profile (AutoMapper) and create maps
    ```
       public class Maps : Profile
       {
        public Maps()
        {
            CreateMap<Class, ClassDTO>().ReverseMap();
        }
       }
    ```
  - Add AutoMapper to services in Startup.cs
    ```
       services.AddAutoMapper(typeof(Maps));
    ```
9. Create DTO folder for dto files
10. Create a 'base' repository Interface
    ```
       public interface IRepositoryBase<T> where T : class
       {
            Task<IList<T>> FindAll();
            Task<T> FindById(int id);
            Task<bool> Create(T model);
            Task<bool> Update(T model);
            Task<bool> Delete(T model);
            Task<bool> Save();
       }
    ```
11. Create an Interface for each models, replace 'Model' with models from the project
    ```
        public interface IModelRepository : IRepositoryBase<Model>
        {
        }
    ```
12. Implement the interface in a model repository class
    ```
         public class ModelRepository : IModelRepository
         {
         }
    ```
    and add it to Startup.cs/ConfigureServices()
    ```
        services.AddScoped<IModelRepository, ModelRepository>();
    ```
13. Install and add 'Microsoft.AspNetCore.Mvc.NewtonsoftJson' to the project
    Startup.cs/ConfigureServices()
    ```
        services.AddControllers().AddNewtonsoftJson(opts =>
         opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
    ```
14. Seed Admin user and Roles(add any other roles here if needed) data
    - Create SeedData.cs (Data folder)
        ```
          public static class SeedData
          {
            public async static Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                await SeedRoles(roleManager);
                await SeedUsers(userManager);
            }

            private async static Task SeedUsers(UserManager<IdentityUser> userManager)
            {
                if (await userManager.FindByEmailAsync("admin@admin.com") == null)
                {
                    var user = new IdentityUser
                    {
                        UserName = "admin",
                        Email = "admin@admin.com"
                    };
                    var result = await userManager.CreateAsync(user, "*******");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }

            private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
            {
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    var role = new IdentityRole
                    {
                        Name = "Admin"
                    };
                    await roleManager.CreateAsync(role);
                }
            }
          }
        ```
    - Enable AddRoles in Startup.cs/ConfigureServices()
        ```
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false) //<- If TRUE Confirmation required after register
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
        ```
    - Set SeedData.cs to load on app start in Startup.cs/Configure() (make sure UserManager and RoleManager is passed to the function)
        ```
            public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            UserManager<IdentityUser> userManager, // <- Add UserManager
            RoleManager<IdentityRole> roleManager) // <- Add RoleManager
            {
                SeedData.Seed(userManager, roleManager).Wait();  // .Wait() because Seed() is async
            }
        ```

    