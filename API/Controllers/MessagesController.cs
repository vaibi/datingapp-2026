using System;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MessagesController(IUnitOfWork uow) : BaseAPIController
{
    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
    {
        var sender = await uow.MemberRepository.GetMemberByIDAsync(User.GetMemberId());
        var recipient = await uow.MemberRepository.GetMemberByIDAsync(createMessageDto.RecipientId);

        if(sender == null || recipient == null || sender.Id == createMessageDto.RecipientId)
            return BadRequest("Cannot send this message");

        var message = new Message
        {
            SenderId = sender.Id,
            RecipientId = recipient.Id,
            Content = createMessageDto.Content
        };

        uow.MessageRepository.AddMessage(message);

        if(await uow.Complete()) return message.ToDto();

        return BadRequest("Failed to send message");
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResut<MessageDto>>> GetMessagesByContainer([FromQuery] MessageParams messageParams)
    {
        messageParams.MemberId = User.GetMemberId();

        return await uow.MessageRepository.getMessagesForMember(messageParams);
    }

    [HttpGet("thread/{recipientId}")]
    public async Task<ActionResult<IReadOnlyList<MessageDto>>> getMessageThread(string recipientId)
    {
        return Ok(await uow.MessageRepository.GetMessageThread(User.GetMemberId(), recipientId));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(string id)
    {
        var MemberId = User.GetMemberId();
        var message = await uow.MessageRepository.GetMessage(id);

        if(message == null) return BadRequest("Cannot delete this message"); 

        if(message.SenderId != MemberId && message.RecipientId != MemberId)
            return BadRequest("you cannot delet this message");

        if(message.SenderId == MemberId) message.SenderDeleted = true;
        if(message.RecipientId == MemberId) message.RecipientDeleted = true;

        if(message is {SenderDeleted: true, RecipientDeleted: true})
        {
            uow.MessageRepository.DeleteMessage(message);
        }

        if(await uow.Complete()) return Ok();

        return BadRequest("Problem deleting the message");
    }
}