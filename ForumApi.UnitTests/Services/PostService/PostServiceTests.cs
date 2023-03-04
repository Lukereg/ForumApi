using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ForumApi.Services.PostService;
using ForumApi.Services.PaginationService;
using ForumApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ForumApi.Models.Posts;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moq.EntityFrameworkCore;
using MockQueryable.Moq;
using System.Reflection;
using ForumApi.MapProfiles;

namespace ForumApi.Tests.Services
{
    public class PostServiceTest
    {

        private readonly PostService _postService;
        private readonly Mock<IForumDbContext> _forumDbContext = new();
        private readonly Mock<IPaginationService<Post>> _paginationServiceMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        public PostServiceTest()
        {
            _postService = new PostService(_forumDbContext.Object, _mapperMock.Object, _paginationServiceMock.Object);
        }

        public static IEnumerable<object[]> Data()
        {
            yield return new object[] { "testtitle", "testmessage", 1, new List<int> { 1, 2 }, 1 };
            yield return new object[] { "saddsada", "cxvxcxvc", 1, new List<int> { 3 }, 1 };
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task AddPost_ForValidData_InvokesSaveChangesAsyncAndAddAsync(string title, string message, int authorId, List<int> tagsIds, int categoryId)
        {
            //arange
            var addPostDto = new AddPostDto
            {
                Title = title,
                Message = message,
                AuthorId = authorId,
                TagsIds = tagsIds
            };

            var tags = new List<Tag>
            {
                new Tag { Id = 1, Value = "tag1" },
                new Tag { Id = 2, Value = "tag2" },
                new Tag { Id = 3, Value = "tag3" }
            };

            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Testname",
                    Surname = "Testsurname",
                    Email = "email@gmail.com",
                    Login = "testlogin"
                }
            };

            var categories = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Category"
                }
            };

            var post = new Post
            {
                Title = addPostDto.Title,
                Message = addPostDto.Message,
                AuthorId = addPostDto.AuthorId,
                CategoryId = categoryId
            };

            var posts = new List<Post>();

            var mockDbSetTags = tags.AsQueryable().BuildMockDbSet();
            _forumDbContext.Setup(f => f.Tags).Returns(mockDbSetTags.Object);

            var mockDbSetUsers = users.AsQueryable().BuildMockDbSet();
            _forumDbContext.Setup(f => f.Users).Returns(mockDbSetUsers.Object);

            var mockDbSetCategories = categories.AsQueryable().BuildMockDbSet();
            _forumDbContext.Setup(f => f.Categories).Returns(mockDbSetCategories.Object);

            var mockDbSetPosts = posts.AsQueryable().BuildMockDbSet();
            _forumDbContext.Setup(f => f.Posts).Returns(mockDbSetPosts.Object);

            _mapperMock.Setup(m => m.Map<Post>(addPostDto)).Returns(post);

            //act
            var postId = await _postService.AddPost(categoryId, addPostDto);

            //assert
            _forumDbContext.Verify(f => f.Posts.AddAsync(It.IsAny<Post>(), default), Times.Once);
            _forumDbContext.Verify(f => f.SaveChangesAsync(default), Times.Once);
        }      
    }
}
