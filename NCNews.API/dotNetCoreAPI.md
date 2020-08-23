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
