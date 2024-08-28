using Mapster;
using SimpleBlog.Common.DTOs;
using SimpleBlog.MVC.Validation.ViewModels.Account;
using SimpleBlog.MVC.Validation.ViewModels.Post;

namespace SimpleBlog.MVC.Extensions;

public static class Mappers
{
    #region Parse to dto

    public static AccountDto ParseToDto(this CreatingAccountModel model) => model.Adapt<AccountDto>();

    public static AccountDto ParseToDto(this UpdatingAccountModel model)
    {
        TypeAdapterSetter<UpdatingAccountModel, AccountDto> setter = TypeAdapterConfig<UpdatingAccountModel, AccountDto>
            .NewConfig()
            .Map(dest => dest.AccountId, src => src.Id);
        return model.Adapt<AccountDto>();
    }

    public static PostDto ParseToDto(this CreatingPostModel model) => model.Adapt<PostDto>();

    public static PostDto ParseToDto(this UpdatingPostModel model)
    {
        TypeAdapterSetter<UpdatingPostModel, PostDto> setter = TypeAdapterConfig<UpdatingPostModel, PostDto>
            .NewConfig()
            .Map(dest => dest.PostId, src => src.Id);
        return model.Adapt<PostDto>();
    }

    #endregion

    #region Parse to view model

    public static UpdatingAccountModel ParseToViewModel(this AccountDto accountDto)
    {
        TypeAdapterSetter<AccountDto, UpdatingAccountModel> setter = TypeAdapterConfig<AccountDto, UpdatingAccountModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.AccountId);
        return accountDto.Adapt<UpdatingAccountModel>(setter.Config);
    }
    
    public static PostModel ParseToViewModel(this PostDto postDto)
    {
        TypeAdapterSetter<PostDto, PostModel> setter = TypeAdapterConfig<PostDto, PostModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.PostId);
        return postDto.Adapt<PostModel>(setter.Config); 
    }

    public static UpdatingPostModel ParseToUpdViewModel(this PostDto model)
    {
        TypeAdapterSetter<PostDto, UpdatingPostModel> setter = TypeAdapterConfig<PostDto, UpdatingPostModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.PostId);
        UpdatingPostModel updatingPostModel = model.Adapt<UpdatingPostModel>(setter.Config);
        return updatingPostModel;
    }

    public static List<PostModel> ParseToViewModels(this List<PostDto> models)
    {
        List<PostModel> postModels = [];
        postModels.AddRange(models.Select(model => model.ParseToViewModel()));

        return postModels;
    }

    #endregion
}