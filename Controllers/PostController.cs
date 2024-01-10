using DotnetAPI.data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        public PostController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("Posts")]
        public IEnumerable<Post> GetPosts() 
        {
            string sqlGetPosts = @"
            SELECT [PostId],
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated] 
            FROM DotNetCourseDatabase.TutorialAppSchema.Posts";

            return _dapper.LoadData<Post>(sqlGetPosts);
        }

        [HttpGet("PostSingle/{postId}")]
        public Post GetsinglePost(int postId)
        {
            string sqlGetPosts = @"
            SELECT [PostId],
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated] 
            FROM DotNetCourseDatabase.TutorialAppSchema.Posts
                WHERE PostId = " + postId.ToString();

            return _dapper.LoadDataSingle<Post>(sqlGetPosts);
        }

        [HttpGet("PostsByUser/{userId}")]
        public IEnumerable<Post> GetPostsByUser(int userId)
        {
            string sqlGetPosts = @"
            SELECT [PostId],
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated] 
            FROM DotNetCourseDatabase.TutorialAppSchema.Posts
                WHERE UserId = " + userId.ToString();

            return _dapper.LoadData<Post>(sqlGetPosts);
        }

        [HttpGet("MyPosts")]
        public IEnumerable<Post> GetMyPosts()
        {
            string sqlGetPosts = @"
            SELECT [PostId],
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated] 
            FROM DotNetCourseDatabase.TutorialAppSchema.Posts
                WHERE UserId = " + this.User.FindFirst("userId")?.Value;

            return _dapper.LoadData<Post>(sqlGetPosts);
        }

        [HttpPost("Post")]
        public IActionResult AddPost(PostToAddDto postToAdd)
        {
            string sqlPostToAdd = @"
            INSERT INTO DotNetCourseDatabase.TutorialAppSchema.Posts(
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated]) VALUES (" + this.User.FindFirst("userId")?.Value
                + ", '" + postToAdd.PostTitle
                + "', '" + postToAdd.PostContent
                + "', GETDATE(), GETDATE() )";

            if (_dapper.ExecuteSql(sqlPostToAdd))
            {
                return Ok();
            }

            throw new Exception("Failed to add post");
        }

        [HttpPut("Post")]
        public IActionResult EditPost(PostToEditDto postToEdit)
        {
            string sqlPostToEdit = @"
            UPDATE DotNetCourseDatabase.TutorialAppSchema.Posts
            SET
                PostContent = '" + postToEdit.PostContent 
                + "', PostTitle = '" + postToEdit.PostTitle 
                + @"', PostUpdated = GETDATE() 
                    WHERE PostId = " + postToEdit.PostId.ToString() +
                    "AND UserId = " + this.User.FindFirst("userId")?.Value;

            if (_dapper.ExecuteSql(sqlPostToEdit))
            {
                return Ok();
            }

            throw new Exception("Failed to edit post");
        }

        [HttpDelete("Post/{postId}")]
        public IActionResult DeletePost(int postId)
        {
            string sqlDeletePost = "DELETE FROM DotNetCourseDatabase.TutorialAppSchema.Posts WHERE PostId = " + postId.ToString()
            + "AND UserId = " + this.User.FindFirst("userId")?.Value;

            if (_dapper.ExecuteSql(sqlDeletePost))
            {
                return Ok();
            }

            throw new Exception("Failed to delete post!");
        }

        [HttpGet("PostsBySearch/{searchParam}")]
        public IEnumerable<Post> PostsBySearch(string searchParam)
        {
            string sqlGetPosts = @"
            SELECT [PostId],
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated] 
            FROM DotNetCourseDatabase.TutorialAppSchema.Posts
                WHERE PostTitle LIKE '% " + searchParam + "%'" +
                " OR PostContent LIKE '% " + searchParam + "%'";

            return _dapper.LoadData<Post>(sqlGetPosts);
        }

    }
}