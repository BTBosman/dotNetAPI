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

        [HttpGet("Posts/{postId}/{userId}/{searchParam}")]
        public IEnumerable<Post> GetPosts(int postId = 0, int userId = 0, string searchParam = "none")
        {
            string sqlGetPosts = @"EXEC TutorialAppSchema.spPosts_Get";
            string parameters = "";

            if (postId != 0)
            {
                parameters += ", @PostId=" + postId.ToString();
            }

            if (userId != 0)
            {
                parameters += ", @UserId=" + userId.ToString();
            }

            if (searchParam.ToLower() != "none")
            {
                parameters += ", @SearchValue='" + searchParam + "'";
            }

            if (parameters.Length > 0)
            {
                sqlGetPosts += parameters.Substring(1);
            }

            Console.WriteLine(sqlGetPosts);

            return _dapper.LoadData<Post>(sqlGetPosts);
        }

        [HttpGet("MyPosts")]
        public IEnumerable<Post> GetMyPosts()
        {
            string sqlGetPosts = @"EXEC TutorialAppSchema.spPosts_Get
                @UserId = " + this.User.FindFirst("userId")?.Value;

            return _dapper.LoadData<Post>(sqlGetPosts);
        }

        [HttpPut("UpsertPost")]
        public IActionResult UpsertPost(Post postToUpsert)
        {
            string sqlPostToAdd = @"
                EXEC TutorialAppSchema.spPosts_Upsert
                @UserId=" + this.User.FindFirst("userId")?.Value
                + ", @PostTitle = '" + postToUpsert.PostTitle
                + "', @PostContent='" + postToUpsert.PostContent + "'";

            if (postToUpsert.PostId > 0)
            {

                sqlPostToAdd += ", @PostId = " + postToUpsert.PostId;
            }


            if (_dapper.ExecuteSql(sqlPostToAdd))
            {
                return Ok();
            }

            throw new Exception("Failed to upsert a post");
        }

        [HttpDelete("Post/{postId}")]
        public IActionResult DeletePost(int postId)
        {
            string sqlDeletePost = "EXEC TutorialAppSchema.spPost_Delete @PostId = " + postId.ToString()
            + ", @UserId = " + this.User.FindFirst("userId")?.Value;

            if (_dapper.ExecuteSql(sqlDeletePost))
            {
                return Ok();
            }

            throw new Exception("Failed to delete post!");
        }
    }
}