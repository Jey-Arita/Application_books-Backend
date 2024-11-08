using Application_books.Constants;
using Application_books.Database.Entities;
using Application_books.Database.Entitties;
using Application_books.Dtos.Usuarios;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace Application_books.Database
{
    public class ApplicationBooksSeeder
    {
        public static async Task LoadDataAsync(
            ApplicationbooksContext context,
            ILoggerFactory loggerFactory,
            UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            try
            {
                await LoadRolesAndUsersAsync(userManager, roleManager, loggerFactory);
                await LoadAutorAsync(loggerFactory, context);
                await LoadLibrosAsync(loggerFactory, context);                             
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<ApplicationBooksSeeder>();
                logger.LogError(e, "Error inicializando la data del API");
            }
        }

        public static async Task LoadRolesAndUsersAsync(
            UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            ILoggerFactory loggerFactory
            )
        {
            try
            {
                if (!await roleManager.Roles.AnyAsync())
                {
                    await roleManager.CreateAsync(new IdentityRole(RolesConstant.ADMIN));
                    await roleManager.CreateAsync(new IdentityRole(RolesConstant.SUSCRIPTOR));
                    await roleManager.CreateAsync(new IdentityRole(RolesConstant.VISIT));
                }

                if (!await userManager.Users.AnyAsync())
                {
                    var userAdmin = new UserEntity
                    {
                        FirstName = "Administrador",
                        LastName = "Blog",
                        Email = "admin@blogunah.edu",
                        UserName = "admin@blogunah.edu",
                    };

                    var userAuthor = new UserEntity
                    {
                        FirstName = "Sucriptor",
                        LastName = "Blog",
                        Email = "Suscriptor@blogunah.edu",
                        UserName = "Suscriptor@blogunah.edu",
                    };

                    var normalUser = new UserEntity
                    {
                        FirstName = "Visit",
                        LastName = "Blog",
                        Email = "Visit@blogunah.edu",
                        UserName = "Visit@blogunah.edu",
                    };

                    await userManager.CreateAsync(userAdmin, "Temporal01*");
                    await userManager.CreateAsync(userAuthor, "Temporal01*");
                    await userManager.CreateAsync(normalUser, "Temporal01*");

                    await userManager.AddToRoleAsync(userAdmin, RolesConstant.ADMIN);
                    await userManager.AddToRoleAsync(userAuthor, RolesConstant.SUSCRIPTOR);
                    await userManager.AddToRoleAsync(normalUser, RolesConstant.VISIT);
                }
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<ApplicationBooksSeeder>();
                logger.LogError(e.Message);
            }


        }

        public static async Task LoadLibrosAsync(ILoggerFactory loggerFactory, ApplicationbooksContext _context)
        {
            try
            {
                var jsonfilePath = "SeedData/libros.json";
                var jsonContent = await File.ReadAllTextAsync(jsonfilePath);
                var libros = JsonConvert.DeserializeObject<List<LibroEntity>>(jsonContent);
                if (!await _context.Libros.AnyAsync())
                {
                    for (int i = 0; i < libros.Count; i++)
                    {
                        libros[i].FechaCreacion = DateTime.Now;
                    }

                    _context.Libros.AddRange(libros);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<ApplicationbooksContext>();
                logger.LogError(e, "Error al ejecutar el Seed de libros.");
            }
        }
        public static async Task LoadAutorAsync(ILoggerFactory loggerFactory, ApplicationbooksContext _context)
        {
            try
            {
                var jsonfilePath = "SeedData/autores.json";
                var jsonnContent = await File.ReadAllTextAsync(jsonfilePath);
                var autores = JsonConvert.DeserializeObject<List<AutorEntity>>(jsonnContent);
                if (!await _context.Autores.AnyAsync())
                {
                    _context.Autores.AddRange(autores);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<ApplicationbooksContext>();
                logger.LogError(e, "Error al ejecutar el Seed de autores.");
            }
        }
    }
}