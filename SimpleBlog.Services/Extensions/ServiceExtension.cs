using Microsoft.Extensions.DependencyInjection;
using SimpleBlog.Common.DTOs;
using SimpleBlog.Data.Entities;
using SimpleBlog.Data.Repositories;
using SimpleBlog.Data.Repositories.Abstract;
using SimpleBlog.Services.Services;
using SimpleBlog.Services.Services.Abstract;

namespace SimpleBlog.Services.Extensions;

public static class ServiceExtension
{
    public static void AddDbServices(this IServiceCollection collection)
    {
        collection.AddScoped<IAccountRepository, AccountRepository>();
        collection.AddScoped<IPostRepository, PostRepository>();
        collection.AddScoped<ILikeRepository, LikeRepository>();
        collection.AddScoped<IPostLikeRepository, PostLikeRepository>();

        collection.AddScoped<IAccountService, AccountService>();
        collection.AddScoped<ILikeService, LikeService>();
        collection.AddScoped<IPostLikeService, PostLikeService>();
        collection.AddScoped<IPostService, PostService>();
    }
}