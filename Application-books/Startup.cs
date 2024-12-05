using Application_books.Database;
using Application_books.Database.Entities;
using Application_books.Database.Entitties;
using Application_books.Helpers;
using Application_books.Services;
using Application_books.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Application_books
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //Add Custom services
            services.AddDbContext<ApplicationbooksContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IAutorServices, AutoresServices>();
            services.AddTransient<ILibrosServices, LibrosServices>();
            services.AddTransient<ICalificacionesServices, CalificacionesServices>();
            services.AddTransient<IUsuariosServices, UsuariosServices>();
            services.AddTransient<IMembresiaServicio, MembresiaServices>();
            services.AddTransient<IListaFavoritoServices, ListaFavoritosServices>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IComentariosServices, ComentariosService>();
            services.AddTransient<IAuditService, AuditService>();
            services.AddTransient<IGeneroService,GenerosServices>();
            services.AddTransient<IDashboardService, DashboardService>();

            // Add Identity
            services.AddIdentity<UserEntity, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            }).AddEntityFrameworkStores<ApplicationbooksContext>()
              .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });


            // Configurar AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddCors(opt =>
            {
                var allowsURLS = Configuration.GetSection("AllowURLS").Get<string[]>();
                opt.AddPolicy("CorsPolicy", builder => builder
                .WithOrigins(allowsURLS)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // Aqui se sgrega de la conexion
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
