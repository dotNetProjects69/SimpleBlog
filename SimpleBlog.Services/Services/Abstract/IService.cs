namespace SimpleBlog.Services.Services.Abstract;

public interface IService<TDto>
{
    Task<List<TDto>> GetAllModels();

    Task<TDto?> GetModelById(int id);

    Task<TDto> AddModel(TDto model);

    Task<TDto?> UpdateModel(TDto model);

    Task<string> DeleteModel(int id);
}