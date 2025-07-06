using Eventra.Shared.DTO.Events;
using Eventra.UserMicroservice.Application.Services.Interfaces;
using MassTransit;

namespace Eventra.UserMicroservice.Application.Services
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MessagePublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishUserRegisteredAsync(UserRegisteredEvent message)
        {
            try
            {
                await _publishEndpoint.Publish(message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
