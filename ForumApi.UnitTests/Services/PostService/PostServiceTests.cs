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
using Xunit.Sdk;
using Microsoft.Extensions.Hosting;
using ForumApi.Exceptions;
using System.ComponentModel;
using ForumApi.Models.Queries;

namespace ForumApi.Tests.Services
{
    public class PostServiceTest
    {

        private readonly PostService _postService;
        private readonly Mock<IForumDbContext> _forumDbContextMock = new();
        private readonly Mock<IPaginationService<Post>> _paginationServiceMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        public PostServiceTest()
        {
            _postService = new PostService(_forumDbContextMock.Object, _mapperMock.Object, _paginationServiceMock.Object);
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
            //arrange
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
            _forumDbContextMock.Setup(f => f.Tags).Returns(mockDbSetTags.Object);

            var mockDbSetUsers = users.AsQueryable().BuildMockDbSet();
            _forumDbContextMock.Setup(f => f.Users).Returns(mockDbSetUsers.Object);

            var mockDbSetCategories = categories.AsQueryable().BuildMockDbSet();
            _forumDbContextMock.Setup(f => f.Categories).Returns(mockDbSetCategories.Object);

            var mockDbSetPosts = posts.AsQueryable().BuildMockDbSet();
            _forumDbContextMock.Setup(f => f.Posts).Returns(mockDbSetPosts.Object);

            _mapperMock.Setup(m => m.Map<Post>(addPostDto)).Returns(post);

            //act
            var postId = await _postService.AddPost(categoryId, addPostDto);

            //assert
            _forumDbContextMock.Verify(f => f.Posts.AddAsync(It.IsAny<Post>(), default), Times.Once);
            _forumDbContextMock.Verify(f => f.SaveChangesAsync(default), Times.Once);
        }

        [Theory]
        [InlineData(1, "testTitle", "testMessage", 1, 1, "2015-05-16T05:50:06")]
        [InlineData(5, "asdcx", "xadfsdfg", 7, 15, "2022-08-18T05:50:04")]
        public async Task GetPostById_IdExists_ReturnGetPostDto(int id, String title, string message, int authorId, int categoryId, DateTime createdDate)
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

            var getPostDto = new GetPostDto
            {
                Id = id,
                Title = title,
                Message = message,
                AuthorId = authorId,
                CategoryId = categoryId,
                CreatedDate = createdDate
            };

            var categories = new List<Category>
            {
                new Category
                {
                    Id = categoryId,
                    Name = "Category"
                }
            };

            var mockDbSetCategories = categories.AsQueryable().BuildMockDbSet();
            _forumDbContextMock.Setup(f => f.Categories).Returns(mockDbSetCategories.Object);

            var posts = new List<Post>();
            posts.Add(post);
            var mockDbSetPosts = posts.AsQueryable().BuildMockDbSet();

            _forumDbContextMock.Setup(f => f.Posts).Returns(mockDbSetPosts.Object);

            _mapperMock.Setup(m => m.Map<GetPostDto>(post)).Returns(getPostDto);

            //act
            var result = await _postService.GetPostById(categoryId, id);

            //assert
            result.Id.Should().Be(post.Id);
            result.CategoryId.Should().Be(post.CategoryId);
            result.CreatedDate.Should().Be(post.CreatedDate);
            result.Message.Should().Be(post.Message);
            result.Title.Should().Be(post.Title);
            result.AuthorId.Should().Be(post.AuthorId);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(5, 4)]
        public async Task GetPostById_IdDoesNotExist_ThrowsNotFoundException(int categoryId, int id)
        {
            //arrange
            var categories = new List<Category>
            {
                new Category
                {
                    Id = categoryId,
                    Name = "Category"
                }
            };

            var mockDbSetCategories = categories.AsQueryable().BuildMockDbSet();
            _forumDbContextMock.Setup(f => f.Categories).Returns(mockDbSetCategories.Object);

            var posts = new List<Post>();
            var mockDbSetPosts = posts.AsQueryable().BuildMockDbSet();
            _forumDbContextMock.Setup(f => f.Posts).Returns(mockDbSetPosts.Object);

            //act
            var action = async () => await _postService.GetPostById(categoryId, id);

            //assert
            await action.Should().ThrowExactlyAsync<NotFoundException>();
        }

        [Theory]
        [InlineData(1, "testTitle", "testMessage", 1, 1, "2015-05-16T05:50:06")]
        [InlineData(5, "asdcx", "xadfsdfg", 7, 15, "2022-08-18T05:50:04")]
        public async Task GetPostById_CategoryIdDoesNotExist_ThrowsNotFoundException(int id, String title, string message, int authorId, int categoryId, DateTime createdDate)
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

            var getPostDto = new GetPostDto
            {
                Id = id,
                Title = title,
                Message = message,
                AuthorId = authorId,
                CategoryId = categoryId,
                CreatedDate = createdDate
            };

            var categories = new List<Category>();
            var mockDbSetCategories = categories.AsQueryable().BuildMockDbSet();
            _forumDbContextMock.Setup(f => f.Categories).Returns(mockDbSetCategories.Object);

            var posts = new List<Post>();
            posts.Add(post);
            var mockDbSetPosts = posts.AsQueryable().BuildMockDbSet();
            _forumDbContextMock.Setup(f => f.Posts).Returns(mockDbSetPosts.Object);

            //act
            var action = async () => await _postService.GetPostById(categoryId, id);

            //assert
            await action.Should().ThrowExactlyAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetPosts_PostsExist_ReturnsPostsCollection()
        {
            //arrange
            int categoryId = 1;

            var posts = new List<Post>
            {
                new Post {Id = 1, CategoryId = categoryId, AuthorId = 1, Title = "TestTitle", Message = "TestMessage", CreatedDate = DateTime.Now},
                new Post {Id = 2, CategoryId = categoryId, AuthorId = 3, Title = "TestttttTitleeee", Message = "TestttttMessageeeee", CreatedDate = DateTime.Now}
            };

            var postsDto = new List<GetPostDto>
            {
                new GetPostDto {Id = 1, CategoryId = categoryId, AuthorId = 1, Title = "TestTitle", Message = "TestMessage", CreatedDate = posts[0].CreatedDate},
                new GetPostDto {Id = 2, CategoryId = categoryId, AuthorId = 3, Title = "TestttttTitleeee", Message = "TestttttMessageeeee", CreatedDate = posts[1].CreatedDate}
            };

            var categories = new List<Category>
            {
                new Category { Id = categoryId, Name = "Category" }
            };


            var mockDbSetPosts = posts.AsQueryable().BuildMockDbSet();
            _forumDbContextMock.Setup(f => f.Posts).Returns(mockDbSetPosts.Object);

            var mockDbSetCategories = categories.AsQueryable().BuildMockDbSet();
            _forumDbContextMock.Setup(f => f.Categories).Returns(mockDbSetCategories.Object);

            _mapperMock.Setup(m => m.Map<List<GetPostDto>>(posts)).Returns(postsDto);

            var paginationQuery = new PaginationQuery
            {
                PageNumber = 1, PageSize = posts.Count
            };

            //act
            var result = await _postService.GetPosts(categoryId, paginationQuery);

            //assert
            result.TotalItemsCount.Should().Be(posts.Count);
        }

    }
}
