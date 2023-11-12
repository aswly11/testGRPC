using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testGRPC.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string  Title { get; set; }
        public string  Desctiption { get; set; }
        public string  Status { get; set; } = "New";

    }
}