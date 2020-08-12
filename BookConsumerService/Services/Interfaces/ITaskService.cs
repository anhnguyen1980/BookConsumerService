using BookConsumerService.Entities;
using BookConsumerService.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookConsumerService.Services.Interfaces
{
   public interface ITaskService
    {
        Task<TaskDto> GetTask(Guid id);
        Task<IEnumerable<TaskDto>> GetTasks(string strFind);
        Task<bool> InsertTask(TaskHistory taskHistory);
        Task<bool> UpdateTask(TaskHistory taskHistory);
        Task<bool> DeleteTask(Guid id);
    }
}

