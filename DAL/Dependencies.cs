using DAL.Repositories;
using DAL.UOW;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DAL.IRepositories;

namespace DAL
{
    public static class Dependencies
    {
        public static void RegisterDependencies(this IServiceCollection services, string con)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IRepository<BookAuthor>,Repository<BookAuthor>>();
            services.AddScoped<IRepository<BookGenre>,Repository<BookGenre>>();
            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            
            Console.WriteLine("Dependency Injection from DAL");
            services.AddDbContext<AppContext>(option => option.UseSqlServer(con));
        }
    }
}
