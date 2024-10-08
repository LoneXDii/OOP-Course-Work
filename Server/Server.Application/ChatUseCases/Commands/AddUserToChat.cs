﻿namespace Server.Application.ChatUseCases.Commands;

public sealed record AddUserToChatRequest(int userId, int chatId) : IRequest { }

internal class AddUserToChatRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddUserToChatRequest> {
    public async Task Handle(AddUserToChatRequest request, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId, cancellationToken);
        var chat = await unitOfWork.ChatRepository.GetByIdAsync(request.chatId, cancellationToken, c => c.Users);
        if (user is null || chat is null) return;
        if (chat.Users.Contains(user))
        {
            throw new Exception("User is already in this chat");
        }
        chat.Users.Add(user);
        await unitOfWork.SaveAllAsync();
    }
}
