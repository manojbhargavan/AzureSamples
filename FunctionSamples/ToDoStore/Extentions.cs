using System;

namespace ToDoStore
{
    public static class Extentions
    {
        public static TodoEntity MapTodoToEntity(this TodoRequestModel source)
        {
            return new TodoEntity()
            {
                PartitionKey = "Todo",
                RowKey = source.Id,
                CreatedTime = source.CreatedTime,
                TaskDescription = source.TaskDescription,
                UpdatedTime = source.UpdatedTime,
                IsCompleted = source.IsCompleted,
                Timestamp = DateTime.Now
            };
        }

        public static TodoRequestModel MapEntityToTodo(this TodoEntity source)
        {
            return new TodoRequestModel()
            {
                Id = source.RowKey,
                CreatedTime = source.CreatedTime,
                TaskDescription = source.TaskDescription,
                UpdatedTime = source.UpdatedTime,
                IsCompleted = source.IsCompleted
            };
        }

    }
}
