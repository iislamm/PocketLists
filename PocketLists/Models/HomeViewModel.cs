using System;
using System.Linq;
using System.Threading.Tasks;
using GenericCollections = System.Collections.Generic;


namespace PocketLists.Models
{
    public class HomeViewModel
    {
        public GenericCollections.List<Task> Tasks { get; set; }
        public Task NewTask { get; set; }
    }
}
