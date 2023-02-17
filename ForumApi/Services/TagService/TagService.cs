using AutoMapper;
using ForumApi.Entities;
using ForumApi.Exceptions;
using ForumApi.Models.Tags;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.Services.TagService
{
    public class TagService : ITagService
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public TagService(ForumDbContext forumDbContext, IMapper mapper)
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
    }
}
