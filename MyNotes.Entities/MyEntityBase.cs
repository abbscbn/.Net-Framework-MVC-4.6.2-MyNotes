using System;

namespace MyNotes.Entities
{
    public class MyEntityBase
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime ModifedOn { get; set; }

        public string ModifiedUsername { get; set; }


    }
}
