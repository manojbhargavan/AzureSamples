using Newtonsoft.Json;
using System;

namespace ToDoStore
{
    public class TodoRequestModel
    {
        public string Id { get; set; }
        public string TaskDescription { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsCompleted { get; set; }
        public TodoRequestModel()
        {
            Id = Guid.NewGuid().ToString("n");
            CreatedTime = DateTime.Now;
            UpdatedTime = DateTime.Now;
            IsCompleted = false;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
