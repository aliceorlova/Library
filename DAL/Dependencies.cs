﻿using DAL.Repositories;
using DAL.UOW;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL
{
    public static class Dependencies
    {
        public static void RegisterDependencies(this IServiceCollection services, string con)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepository<Author>, Repository<Author>>();
            services.AddScoped<IRepository<Genre>, Repository<Genre>>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IRepository<BookAuthor>,Repository<BookAuthor>>();
            services.AddScoped<IRepository<BookGenre>,Repository<BookGenre>>();
            Console.WriteLine("Dependency Injection from DAL");
            services.AddDbContext<AppContext>(option => option.UseSqlServer(con));

        }
    }
}
