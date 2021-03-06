using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TalentManagement.Application.Exceptions;
using TalentManagement.Application.Interfaces.Repositories;
using TalentManagement.Application.Wrappers;

namespace TalentManagement.Application.Features.Positions.Commands.UpdatePosition
{
    public class UpdatePositionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Salary { get; set; }
        public class UpdatePositionCommandHandler : IRequestHandler<UpdatePositionCommand, Response<int>>
        {
            private readonly IPositionRepositoryAsync _positionRepository;
            public UpdatePositionCommandHandler(IPositionRepositoryAsync positionRepository)
            {
                _positionRepository = positionRepository;
            }
            public async Task<Response<int>> Handle(UpdatePositionCommand command, CancellationToken cancellationToken)
            {
                var position = await _positionRepository.GetByIdAsync(command.Id);

                if (position == null)
                {
                    throw new ApiException($"Position Not Found.");
                }
                else
                {
                    position.PositionTitle = command.Title;
                    position.PositionSalary = command.Salary;
                    position.PositionDescription = command.Description;
                    await _positionRepository.UpdateAsync(position);
                    return new Response<int>(position.Id);
                }
            }
        }
    }
}
