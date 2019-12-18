
using System;
using DAL;
using BLL.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BLL.IServices;

namespace BLL
{
    static public class Dependencies
    {
        public static void RegisterDependencies(this IServiceCollection services, string con)
        {
            services.AddAutoMapper();
            DAL.Dependencies.RegisterDependencies(services, con);
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookingService, BookingService>();
            Console.WriteLine("Dependency Injection from BLL");
        }
    }
}
