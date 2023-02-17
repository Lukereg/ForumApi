using ForumApi.Models.Tags;

namespace ForumApi.Services.TagService
{
    public interface ITagService
    {
        public Task<int> AddTag(AddTagDto addTagDto);
        public Task<IEnumerable<GetTagDto>> GetAllTags();
    }
}
