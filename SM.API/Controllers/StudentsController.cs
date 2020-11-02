using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SM.Application.Common.Models;
using SM.Application.Groups.Queries;
using SM.Application.Students.Commands.AddGroups;
using SM.Application.Students.Commands.CreateStudent;
using SM.Application.Students.Commands.DeleteStudent;
using SM.Application.Students.Commands.UpdateStudent;
using SM.Application.Students.Queries;
using SM.Application.Students.Queries.GetStudent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SM.API.Controllers
{
    [Authorize]
    public class StudentsController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<PagedResponse<IEnumerable<StudentDto>>>> Get([FromQuery] GetStudentsQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<StudentDto>>> Get(long id)
        {
            return await Mediator.Send(new GetStudentByIdQuery { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Response<long>>> Create(CreateStudentCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, UpdateStudentCommand command)
        {
            command.Id = id;

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await Mediator.Send(new DeleteStudentCommand { Id = id });

            return NoContent();
        }

        [HttpGet("{id}/groups")]
        public async Task<ActionResult<PagedResponse<IEnumerable<StudentGroupsListDto>>>> GetGroups(long id, [FromQuery] GetStudentGroupsQuery query)
        {
            query.StudentId = id;

            return await Mediator.Send(query);
        }

        [HttpPost("{id}/groups")]
        public async Task<ActionResult> AddStudents(long id, AddGroupsCommand command)
        {
            if (id != command.StudentId)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}/groups")]
        public async Task<ActionResult> Delete(long id, DeleteGroupsCommand command)
        {
            if (id != command.StudentId)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }
    }
}
