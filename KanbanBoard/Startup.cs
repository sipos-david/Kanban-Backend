using KanbanBoard.DAL.EfDbContext;
using KanbanBoard.DAL.Repositories;
using KanbanBoard.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace KanbanBoard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddDbContext<KanbanBoardDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("KanbanBoardContext")));

            // megzavarja a a token-t :,(
            //services.AddIdentity<UserDto, IdentityRole>()
            //    .AddEntityFrameworkStores<KanbanBoardDbContext>()
            //    .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // base-address of your identityserver
                    options.Authority = "https://localhost:5443";

                    // if you are using API resources, you can specify the name here
                    options.Audience = "kanbanboard";

                    // IdentityServer emits a typ header by default, recommended extra check
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidTypes = new[] { "at+jwt" }
                    };
                });


            // Add repositories
            services.AddScoped<IColumnRepository, ColumnRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ITableRepository, TableRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Add services
            services.AddTransient<IColumnService, ColumnService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<ITableService, TableService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<IUserService, UserService>();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("MyPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
