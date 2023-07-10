using CapstoneProjectToDoApplication.Database.DbConnection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProjectToDoApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<ToDoDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DatabaseTODO")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=ToDoLists}/{action=Index}/{id?}");
            });
        }
    }
}
