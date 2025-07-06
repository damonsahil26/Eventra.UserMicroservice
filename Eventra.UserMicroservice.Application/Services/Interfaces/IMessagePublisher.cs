using Eventra.Shared.DTO.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventra.UserMicroservice.Application.Services.Interfaces
{
    public interface IMessagePublisher
    {
        public Task PublishUserRegisteredAsync(UserRegisteredEvent message);
    }
}
