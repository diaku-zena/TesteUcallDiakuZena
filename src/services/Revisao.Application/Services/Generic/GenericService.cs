using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Revisao.Domain.Interfaces;
using Revisao.Domain.Shared.Entities;

namespace Revisao.Application.Services.Generic;
public class GenericService<TModel, TDTO> : IGenericService<TDTO>
    where TModel : BaseEntity
    where TDTO : class
{
    private readonly IGenericRepository<TModel>? _repository;
    private readonly IMapper? _mapper;

    public GenericService(IGenericRepository<TModel> repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<TDTO> AddAsync(TDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<TModel>(dto);
        var addedEntity = await _repository.AddAsync(entity);
        return _mapper.Map<TDTO>(addedEntity);
    }

    public async Task<bool> DeleteAsync(TDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<TModel>(dto);
        return await _repository.DeleteAsync(entity);
    }

    public async Task<IEnumerable<TDTO>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<TDTO>>(entities);
    }

    public async Task<TDTO> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            throw new KeyNotFoundException($"Entity with ID {id} not found.");

        return _mapper.Map<TDTO>(entity);
    }

    public async Task<bool> UpdateAsync(TDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<TModel>(dto);
        return await _repository.UpdateAsync(entity);
    }
}
