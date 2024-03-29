﻿using SuperShop.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SuperShop.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SuperShop.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random; 

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }


        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Costumer");


            if (!_context.Countries.Any())
            {
                var cities = new List<City>();

                cities.Add(new City { Name = "Lisboa" });
                cities.Add(new City { Name = "Porto" });
                cities.Add(new City { Name = "Faro" });
                cities.Add(new City { Name = "Montijo" });

                _context.Countries.Add(new Country
                {
                    Cities = cities,
                    Name = "Portugal"
                });

                await _context.SaveChangesAsync();
            }

            var user = await _userHelper.GetUserByEmailAsync("f92ferreira@gmail.com");
            
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Filipe",
                    LastName = "Ferreira",
                    Email = "f92ferreira@gmail.com",
                    UserName = "f92ferreira@gmail.com",
                    PhoneNumber = "934093762",
                    CityId = _context.Countries.FirstOrDefault().Cities.LastOrDefault().Id,
                    Address = "Avenida Fialho Gouveia, Nº165",
                    City = _context.Countries.FirstOrDefault().Cities.LastOrDefault(),
                };

                var result = await _userHelper.AddUserAsync(user,"Cinel123");
                
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            if (!_context.Products.Any())
            {
                AddProduct("iphone X",user);
                AddProduct("Magic Mouse",user);
                AddProduct("iWatch Series 4",user);
                AddProduct("iPad",user);
                await _context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name, User user)
        {
            _context.Products.Add(new Product
            {
                Name = name,
                Price = _random.Next(1001),
                IsAvailable = true,
                Stock = _random.Next(100),
                User = user
            }); 
        }
    }
}
