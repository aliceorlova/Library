﻿using DAL.Repositories;
using DAL.UOW;
using Microsoft.Extensions.DependencyInjection;
using System;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using DAL.IRepositories;
using Microsoft.AspNetCore.Identity;


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
            services.AddScoped<IRepository<BookAuthor>, Repository<BookAuthor>>();
            services.AddScoped<IRepository<BookGenre>, Repository<BookGenre>>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            Console.WriteLine("Dependency Injection from DAL");

            services.AddDbContext<AppContext>(option => option.UseSqlServer(con));
            services.AddIdentity<AppUser, IdentityRole<int>>().AddEntityFrameworkStores<AppContext>().AddDefaultTokenProviders();
        }
    }
}
