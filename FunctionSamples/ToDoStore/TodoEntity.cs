using Microsoft.Azure.Cosmos.Table;
using System;

namespace ToDoStore
{
    public class TodoEntity : TableEntity
    {
        public string TaskDescription { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsCompleted { get; set; }
    }
}
