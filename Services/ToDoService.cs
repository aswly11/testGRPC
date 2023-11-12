using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using testGRPC.Data;
using testGRPC.Models;

namespace testGRPC.Services
{
    public class ToDoService : ToDoIt.ToDoItBase
    {
        private readonly AppDBContext _context;

        public ToDoService(AppDBContext context)
        {
            _context = context;
        }

        public override async Task<CreateToDoResponse> CreateToDo(
            CreateToDoRequest request,
            ServerCallContext context
        )
        {
            if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Description))
            {
                throw new RpcException(
                    new Status(
                        StatusCode.InvalidArgument,
                        "Please provide a title and description."
                    )
                );
            }
            ToDoItem toDoItem = new ToDoItem
            {
                Title = request.Title,
                Desctiption = request.Description
            };
            await _context.toDoItems.AddAsync(toDoItem);
            await _context.SaveChangesAsync();
            return await Task.FromResult(new CreateToDoResponse() { Id = toDoItem.Id });
        }

        public override async Task<ReadToDoResponse> ReadToDo(
            ReadToDoRequest request,
            ServerCallContext context
        )
        {
            if (request.Id <= 0)
            {
                throw new RpcException(
                    new Status(StatusCode.InvalidArgument, "Please provide a Task Id.")
                );
            }
            var task = await _context.toDoItems.FindAsync(request.Id);
            if (task != null)
            {
                return await Task.FromResult(
                    new ReadToDoResponse()
                    {
                        Id = task.Id,
                        Description = task.Desctiption,
                        Title = task.Title,
                        ToDoStatus = task.Status
                    }
                );
            }

            throw new RpcException(
                new Status(StatusCode.NotFound, "The task with this Id not found.")
            );
        }

        public override async Task<GetAllResponse> ListToDo(
            GetAllRequest request,
            ServerCallContext context
        )
        {
            var result = new GetAllResponse();
            var toDoItems = await _context.toDoItems.ToListAsync();
            foreach (var task in toDoItems)
            {
                result.ToDo.Add(
                    new ReadToDoResponse
                    {
                        Id = task.Id,
                        Description = task.Desctiption,
                        Title = task.Title,
                        ToDoStatus = task.Status
                    }
                );
            }
            
            return await Task.FromResult(result);
        }

        public override Task<UpdateToDoResponse> UpdateToDo(
            UpdateToDoRequest request,
            ServerCallContext context
        )
        {
            return base.UpdateToDo(request, context);
        }

        public override Task<DeleteToDoResponse> DeleteToDo(
            DeleteToDoRequest request,
            ServerCallContext context
        )
        {
            return base.DeleteToDo(request, context);
        }
    }
}
