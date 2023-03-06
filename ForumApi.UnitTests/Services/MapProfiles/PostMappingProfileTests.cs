using AutoMapper;
using FluentAssertions;
using ForumApi.Entities;
using ForumApi.MapProfiles;
using ForumApi.Models.Posts;
using Microsoft.Extensions.Hosting;
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
        public void CreateMap_AddPostDtoToPost_MapsCorrectly(String title, String message, int authorId)
        {
            //arrange
            var addPostDto = new AddPostDto
            {
                Title = title,
                Message = message
            };

            //act
            var post = _mapper.Map<Post>(addPostDto);

            //assert
            post.Title.Should().Be(addPostDto.Title);
            post.Message.Should().Be(addPostDto.Message);
        }

        [Theory]
        [InlineData(1, "testTitle", "testMessage", 1, 1, "2015-05-16T05:50:06")]
        [InlineData(5, "asdcx", "xadfsdfg", 7, 15, "2022-08-18T05:50:04")]
        public void CreateMap_PostToGetPostDto_MapsCorrectly(int id, String title, string message, int authorId, int categoryId, DateTime createdDate)
        {
            //arrange
            var post = new Post
            {
                Id = id,
                Title = title,
                Message = message,
                AuthorId = authorId,
                CategoryId = categoryId,
                CreatedDate = createdDate
            };

            //act
            var getPostDto = _mapper.Map<GetPostDto>(post);

            //assert
            getPostDto.Id.Should().Be(post.Id);
            getPostDto.Title.Should().Be(post.Title);
            getPostDto.Message.Should().Be(post.Message);
            getPostDto.AuthorId.Should().Be(post.AuthorId);
            getPostDto.CategoryId.Should().Be(post.CategoryId);
            getPostDto.CreatedDate.Should().Be(post.CreatedDate);
        }
    }
}
