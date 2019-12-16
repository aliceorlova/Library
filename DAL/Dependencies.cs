using DAL.Repositories;
using DAL.UOW;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Identity.Core;
using DAL.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace DAL
{
    public static class Dependencies
    {
        public static void RegisterDependencies(this IServiceCollection services, string con)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<IGenreRepository, GenreRepository>();
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IRepository<BookAuthor>, Repository<BookAuthor>>();
            services.AddTransient<IRepository<BookGenre>, Repository<BookGenre>>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IBookingRepository, BookingRepository>();
            Console.WriteLine("Dependency Injection from DAL");
            services.AddDbContext<AppContext>(option => option.UseSqlServer(con), ServiceLifetime.Transient);
            services.AddIdentity<AppUser, IdentityRole<int>>().AddEntityFrameworkStores<AppContext>().AddDefaultTokenProviders();
        }
    }
}
