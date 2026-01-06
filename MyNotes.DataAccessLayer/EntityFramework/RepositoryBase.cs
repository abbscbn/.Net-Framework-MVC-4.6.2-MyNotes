namespace MyNotes.DataAccessLayer.EntityFramework
{
    public class RepositoryBase
    {
        public static DatabaseContext context;

        private static object _lockSync = new object();

        protected RepositoryBase()
        {
            CreateContext();
        }

        public static void CreateContext()
        {
            if (context == null)
            {
                lock (_lockSync)
                {

                    context = new DatabaseContext();

                }
            }
        }
    }
}
