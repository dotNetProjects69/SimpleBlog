using Mapster;
using SimpleBlog.Common.DTOs;
using SimpleBlog.Data.Entities;

namespace SimpleBlog.Services.Extensions;

public static class ParseExtensions
{

    #region Parse To Dto

    public static AccountDto ParseToDto(this Account account) => account.Adapt<AccountDto>();

    public static LikeDto ParseToDto(this Like like) => like.Adapt<LikeDto>();

    public static PostDto ParseToDto(this Post post)
    {
        TypeAdapterSetter<Post, PostDto> setter = TypeAdapterConfig<Post, PostDto>
            .NewConfig()
            .Map(dest => dest.Owner, src => src.Owner);
        return post.Adapt<PostDto>(setter.Config);
    }

    public static PostLikeDto ParseToDto(this PostLike postLike) => postLike.Adapt<PostLikeDto>();
    
    
    public static List<AccountDto> ParseToDto(this List<Account> accounts)
    {
        List<AccountDto> accountDtos = [];

        if (accounts.Count != 0) 
            accountDtos.AddRange(accounts.Select(model => model.ParseToDto()));

        return accountDtos;
    }

    public static List<LikeDto> ParseToModels(this List<Like>? likes)
    {
        List<LikeDto> likeDtos = [];

        if (likes is not null && likes.Count != 0) 
            likeDtos.AddRange(likes.Select(model => model.ParseToDto()));

        return likeDtos;
    }

    public static List<PostDto> ParseToModels(this List<Post>? posts)
    {
        List<PostDto> postDtos = [];

        if (posts is not null && posts.Count != 0) 
            postDtos.AddRange(posts.Select(model => model.ParseToDto()));

        return postDtos;
    }

    public static List<PostLikeDto> ParseToModels(this List<PostLike>? postLikes)
    {
        List<PostLikeDto> postLikeDtos = [];

        if (postLikes is not null && postLikes.Count != 0) 
            postLikeDtos.AddRange(postLikes.Select(model => model.ParseToDto()));

        return postLikeDtos;
    }

    #endregion


    #region Parse To Entity

    public static Account ParseToEntity(this AccountDto account) => account.Adapt<Account>();

    public static Like ParseToEntity(this LikeDto like) => like.Adapt<Like>();

    public static Post ParseToEntity(this PostDto post)
    {
        TypeAdapterSetter<PostDto, Post> setter = TypeAdapterConfig<PostDto, Post>
            .NewConfig()
            .Map(dest => dest.Owner, src => src.Owner.ParseToEntity());
        return post.Adapt<Post>();
    }

    public static PostLike ParseToEntity(this PostLikeDto postLike) => postLike.Adapt<PostLike>();

    #endregion
    
    

}