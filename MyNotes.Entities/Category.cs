using System.Collections.Generic;

namespace MyNotes.Entities
{
    public class Category : MyEntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual List<Note> Notes { get; set; }



    }
}
