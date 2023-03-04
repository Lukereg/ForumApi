using AutoMapper;
using FluentAssertions;
using ForumApi.Entities;
using ForumApi.MapProfiles;
using ForumApi.Models.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApi.Tests.Services.MapProfiles
{
    public class PostMappingProfileTests
    {
        private readonly IMapper _mapper;

        public PostMappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<PostMappingProfile>());
            _mapper = configuration.CreateMapper();
        }

        [Theory]
        [InlineData("TestTitle", "TestMessage", 2)]
        [InlineData("asdadsdas", "fsdvcxxcv", 5)]
        [InlineData("nffgh", "fghe", 7)]
        public void AddPostDto_DtoToPost_MapsCorrectly(String title, String message, int AuthorId)
        {
            //arange
            var addPostDto = new AddPostDto
            {
                Title = title,
                Message = message,
                AuthorId = AuthorId
            };

            //act
            var post = _mapper.Map<Post>(addPostDto);

            //assert
            post.Title.Should().Be(addPostDto.Title);
            post.Message.Should().Be(addPostDto.Message);
            post.AuthorId.Should().Be(addPostDto.AuthorId);
        }
    }
}
