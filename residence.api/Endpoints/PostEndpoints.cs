using residence.application.DTOs;
using residence.application.Interfaces;

namespace residence.api.Endpoints
{

    /// <summary>
    /// Post endpoints
    /// </summary>
    public static class PostEndpoints
    {
        public static void MapPostEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/residences/{residenceId}/posts")
                .WithTags("Posts & Community")
                .WithOpenApi();

            group.MapPost("/", CreatePost)
                .WithName("CreatePost")
                .WithSummary("Create a community post");

            group.MapGet("/{id}", GetPost)
                .WithName("GetPost")
                .WithSummary("Get post by ID");

            group.MapPut("/{id}", UpdatePost)
                .WithName("UpdatePost")
                .WithSummary("Update post");

            group.MapDelete("/{id}", DeletePost)
                .WithName("DeletePost")
                .WithSummary("Delete post");

            group.MapGet("/", GetPostsByResidence)
                .WithName("GetPostsByResidence")
                .WithSummary("Get all community posts");

            group.MapPost("/{postId}/likes", LikePost)
                .WithName("LikePost")
                .WithSummary("Like a post");

            group.MapDelete("/{postId}/likes/{userId}", RemoveLike)
                .WithName("RemoveLike")
                .WithSummary("Remove like from post");

            group.MapPost("/{postId}/comments", AddCommentToPost)
                .WithName("AddCommentToPost")
                .WithSummary("Comment on a post");

            group.MapDelete("/comments/{commentId}", RemoveComment)
                .WithName("RemoveComment")
                .WithSummary("Delete a comment");

            group.MapGet("/{postId}/comments", GetPostComments)
                .WithName("GetPostComments")
                .WithSummary("Get post comments");
        }

        private static async Task<IResult> CreatePost(IPostService service, Guid residenceId, Guid authorId, CreatePostDto dto)
        {
            try
            {
                var result = await service.CreatePostAsync(residenceId, authorId, dto);
                return Results.Created($"/api/residences/{residenceId}/posts/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetPost(IPostService service, Guid id, Guid? currentUserId)
        {
            try
            {
                var result = await service.GetPostByIdAsync(id, currentUserId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
        }

        private static async Task<IResult> UpdatePost(IPostService service, Guid id, UpdatePostDto dto)
        {
            try
            {
                var result = await service.UpdatePostAsync(id, dto);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> DeletePost(IPostService service, Guid id)
        {
            try
            {
                await service.DeletePostAsync(id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetPostsByResidence(IPostService service, Guid residenceId, Guid? currentUserId, [AsParameters] PaginationDto pagination)
        {
            try
            {
                var result = await service.GetPostsByResidenceAsync(residenceId, currentUserId, pagination);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> LikePost(IPostService service, Guid postId, Guid userId)
        {
            try
            {
                var result = await service.LikePostAsync(postId, userId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> RemoveLike(IPostService service, Guid postId, Guid userId)
        {
            try
            {
                await service.RemoveLikeAsync(postId, userId);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> AddCommentToPost(IPostService service, Guid postId, Guid authorId, CreatePostCommentDto dto)
        {
            try
            {
                var result = await service.AddCommentAsync(postId, authorId, dto);
                return Results.Created($"/api/posts/{postId}/comments/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> RemoveComment(IPostService service, Guid commentId)
        {
            try
            {
                await service.RemoveCommentAsync(commentId);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetPostComments(IPostService service, Guid postId, [AsParameters] PaginationDto pagination)
        {
            try
            {
                var result = await service.GetCommentsAsync(postId, pagination);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }
    }
}
