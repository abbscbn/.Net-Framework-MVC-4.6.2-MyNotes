namespace MyNotes.BusinessLayer
{
    public class Test
    {
        public Test()
        {
            DataAccessLayer.DatabaseContext db = new DataAccessLayer.DatabaseContext();
            db.Database.CreateIfNotExists();
        }
    }
}
