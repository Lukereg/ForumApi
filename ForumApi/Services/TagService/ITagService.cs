using ForumApi.Models.Tags;

namespace ForumApi.Services.TagService
{
    public interface ITagService
    {
        public Task<int> AddTag(AddTagDto addTagDto);
        public Task DeleteTag(int id);
        public Task<IEnumerable<GetTagDto>> GetAllTags();
        public Task<GetTagDto> GetTagById(int id);
        public Task UpdateTag(UpdateTagDto updateTagDto, int id);
    }
}
