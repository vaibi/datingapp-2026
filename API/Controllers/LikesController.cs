using System;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController (ILikesRepository likesRepository): BaseAPIController
{
    [HttpPost("{targetMemberId}")]
    public async Task<ActionResult> ToggleLike(string targetMemberId)
    {
        var sourceMemberId = User.GetMemberId();

        if(sourceMemberId == targetMemberId) return BadRequest("Ypu cannot like yourself");

        var existingLike = await likesRepository.GetMemberLike(sourceMemberId, targetMemberId);

        if(existingLike == null)
        {
            var like = new MemberLike
            {
                SourceMemberID = sourceMemberId,
                TargetMemberID = targetMemberId
            };
            likesRepository.AddLike(like);
        }
        else
        {
            likesRepository.DeleteLike(existingLike);
        }

        if(await likesRepository.SaveAllChanges()) return Ok();

        return BadRequest("Failed to update like"); 
    }

    [HttpGet("list")]
    public async Task<ActionResult<IReadOnlyList<string>>> getCurrentMemberLikeIds()
    {
        return Ok(await likesRepository.GetCurrentMemberLikeIds(User.GetMemberId()));
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResut<Member>>> getMemberLikes([FromQuery] LikesParams likesParams)
    {
        likesParams.MemberId = User.GetMemberId();
        var members = await likesRepository.GetMemberLikes(likesParams);
        return Ok(members);
    }
}