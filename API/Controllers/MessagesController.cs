using System;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MessagesController(IMessageRepository messageRepository,
    IMemberRepository memberRepository) : BaseAPIController
{
    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
    {
        var sender = await memberRepository.GetMemberByIDAsync(User.GetMemberId());
        var recipient = await memberRepository.GetMemberByIDAsync(createMessageDto.RecipientId);

        if(sender == null || recipient == null || sender.Id == createMessageDto.RecipientId)
            return BadRequest("Cannot send this message");

        var message = new Message
        {
            SenderId = sender.Id,
            RecipientId = recipient.Id,
            Content = createMessageDto.Content
        };

        messageRepository.AddMessage(message);

        if(await messageRepository.SaveAllAsync()) return message.ToDto();

        return BadRequest("Failed to send message");
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResut<MessageDto>>> GetMessagesByContainer([FromQuery] MessageParams messageParams)
    {
        messageParams.MemberId = User.GetMemberId();

        return await messageRepository.getMessagesForMember(messageParams);
    }

    [HttpGet("thread/{recipientId}")]
    public async Task<ActionResult<IReadOnlyList<MessageDto>>> getMessageThread(string recipientId)
    {
        return Ok(await messageRepository.GetMessageThread(User.GetMemberId(), recipientId));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(string id)
    {
        var MemberId = User.GetMemberId();
        var message = await messageRepository.GetMessage(id);

        if(message == null) return BadRequest("Cannot delete this message"); 

        if(message.SenderId != MemberId && message.RecipientId != MemberId)
            return BadRequest("you cannot delet this message");

        if(message.SenderId == MemberId) message.SenderDeleted = true;
        if(message.RecipientId == MemberId) message.RecipientDeleted = true;

        if(message is {SenderDeleted: true, RecipientDeleted: true})
        {
            messageRepository.DeleteMessage(message);
        }

        if(await messageRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting the message");
    }
}