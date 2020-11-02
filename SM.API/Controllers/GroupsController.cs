using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SM.Application.Common.Models;
using SM.Application.Groups.Commands.AddStudents;
using SM.Application.Groups.Commands.CreateGroup;
using SM.Application.Groups.Commands.DeleteGroup;
using SM.Application.Groups.Commands.UpdateGroup;
using SM.Application.Groups.Queries;
using SM.Application.Groups.Queries.GetGroup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SM.API.Controllers
{
    [Authorize]
    public class GroupsController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<PagedResponse<IEnumerable<GroupDto>>>> Get([FromQuery] GetGroupsQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<GroupDto>>> Get(int id)
        {
            return await Mediator.Send(new GetGroupByIdQuery { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Response<int>>> Create(CreateGroupCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateGroupCommand command)
        {
            command.Id = id;

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteGroupCommand { Id = id });

            return NoContent();
        }

        [HttpGet("{id}/students")]
        public async Task<ActionResult<PagedResponse<IEnumerable<GroupStudentsListDto>>>> GetStudents(int id, [FromQuery] GetGroupStudentsQuery query)
        {
            query.GroupId = id;

            return await Mediator.Send(query);
        }

        [HttpPost("{id}/students")]
        public async Task<ActionResult> AddStudents(int id, AddStudentsCommand command)
        {
            if (id != command.GroupId)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}/students")]
        public async Task<ActionResult> Delete(int id, DeleteStudentsCommand command)
        {
            if (id != command.GroupId)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }
    }
}
