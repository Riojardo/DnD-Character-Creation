using DnD_CharacterCreation.Data;
using DnD_CharacterCreation.Models;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DnD_CharacterCreation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddIdentity<Users, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            var app = builder.Build();

            // Seed roles before app starts
            await SeedRolesAsync(app);

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            /*
                        using var pdf = new PdfDocument(new PdfReader("C:\\Users\\Robin\\source\\repos\\DnD-Character-Creation\\content\\pdf\\5E_CharacterSheet_Fillable.pdf"));
                        var form = PdfAcroForm.GetAcroForm(pdf, false);

                        var fields = form.GetFormFields(); // maintenant disponible
                        foreach (var field in fields)
                        {
                            Console.WriteLine(field.Key); // Nom interne du champ
                        }*/

                       /* string templatePath = "C:\\Users\\Robin\\source\\repos\\DnD-Character-Creation\\content\\pdf\\5E_CharacterSheet_Fillable.pdf";
                        string outputPath = "Content/Gimli_Sheet.pdf";

                        var character = new
                        {
                            Name = "Gimli",
                            ClassLevel = "Fighter 3",
                            Race = "Dwarf",
                            Strength = 16
                        };

                        using var reader = new PdfReader(templatePath);
                        using var writer = new PdfWriter(outputPath);
                        using var pdf = new PdfDocument(reader, writer);

                        var form = PdfAcroForm.GetAcroForm(pdf, true);

                        form.GetField("CharacterName").SetValue(character.Name);
                        form.GetField("ClassLevel").SetValue(character.ClassLevel);
                        form.GetField("Race").SetValue(character.Race);
                        form.GetField("STR").SetValue(character.Strength.ToString());

                        form.FlattenFields(); 

                        pdf.Close();
            */
            app.Run();

            async Task SeedRolesAsync(WebApplication app)
            {
                using var scope = app.Services.CreateScope();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string[] roles = { "Player", "UserAdmin" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
