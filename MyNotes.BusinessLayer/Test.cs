namespace MyNotes.BusinessLayer
{
    public class Test
    {
        public Test()
        {
            using (var db = new DataAccessLayer.DatabaseContext())
            {
                db.Database.Initialize(true);
            }
        }
    }
}
