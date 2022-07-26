﻿using Cookbook.Core.Contracts;
using Cookbook.Core.Models;
using Cookbook.Infrastructure.Data.Models;
using Cookbook.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Core.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IApplicationDbRepository repo;

        public RecipeService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<RecipePreviewViewModel>> GetAllRecipes()
        {
            return await repo.All<Recipe>()
                            .Where(r => r.IsDeleted == false)
                            .Select(r => new RecipePreviewViewModel()
                            {
                                Id = r.Id,
                                Name = r.Name,
                                Picture = r.Picture,
                                Description = string.Join(" ", r.Description.Split().Take(12)),
                                ServeSize = r.ServingSize,
                                AuthorNickname = r.Author.UserName,
                                AuthorId = r.AuthorId
                            })
                            .ToListAsync();
        }

        public async Task<IEnumerable<RecipePreviewViewModel>> GetFilteredRecipes(string keyword)
        {
            return await repo.All<Recipe>()
                .Where(r => r.Name.Contains(keyword))
                .Where(r => r.IsDeleted == false)
                .Select(r => new RecipePreviewViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Picture = r.Picture,
                    Description = string.Join(" ", r.Description.Split().Take(12)),
                    ServeSize = r.ServingSize,
                    AuthorNickname = r.Author.UserName,
                    AuthorId = r.AuthorId
                })
                .ToListAsync();
        }

        public async Task<Recipe> GetRecipeById(string recipeId)
        {
            return await repo.All<Recipe>()
                .FirstOrDefaultAsync(r => r.Id == Guid.Parse(recipeId));
        }
    }
}
