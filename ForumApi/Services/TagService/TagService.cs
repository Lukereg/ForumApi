﻿using AutoMapper;
using ForumApi.Entities;
using ForumApi.Exceptions;
using ForumApi.Models.Categories;
using ForumApi.Models.Tags;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.Services.TagService
{
    public class TagService : ITagService
    {
        private readonly IForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public TagService(IForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<int> AddTag(AddTagDto addTagDto)
        {
            if (await _forumDbContext.Tags.FirstOrDefaultAsync(t => t.Value == addTagDto.Value) != null)
                throw new BadRequestException("The given tag already exists in the database");
            
            var newTag = _mapper.Map<Tag>(addTagDto);
            await _forumDbContext.Tags.AddAsync(newTag);
            await _forumDbContext.SaveChangesAsync();
            return newTag.Id;
        }

        public async Task DeleteTag(int id)
        {
            var tag = await _forumDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (tag is null)
                throw new NotFoundException("Tag not found");

            _forumDbContext.Tags.Remove(tag);
            await _forumDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<GetTagDto>> GetAllTags()
        {
            var tags = await _forumDbContext.Tags
                .AsNoTracking()
                .ToListAsync();
            var result = _mapper.Map<List<GetTagDto>>(tags);
            return result;
        }

        public async Task<GetTagDto> GetTagById(int id)
        {
            var tag = await _forumDbContext.Tags
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
            if (tag is null)
                throw new NotFoundException("Tag not found");

            var result = _mapper.Map<GetTagDto>(tag);
            return result;        
        }

        public async Task UpdateTag(UpdateTagDto updateTagDto, int id)
        {
            var tag = await _forumDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (tag is null)
                throw new NotFoundException("Tag not found");

            tag.Value = updateTagDto.Value;

            await _forumDbContext.SaveChangesAsync();
        }
    }
}
