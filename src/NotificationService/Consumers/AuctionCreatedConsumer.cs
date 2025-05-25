using System;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly ILogger<AuctionCreatedConsumer> _logger;
    private readonly IHubContext<NotificationHub> _hubContext;

    public AuctionCreatedConsumer(ILogger<AuctionCreatedConsumer> logger, IHubContext<NotificationHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
       _logger.LogInformation("--> auction created message received");

        await _hubContext.Clients.All.SendAsync("AuctionCreated", context.Message);
    }
}
