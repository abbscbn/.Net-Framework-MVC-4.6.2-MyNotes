namespace MyNotes.Common
{
    public static class App
    {
        public static ICommon Common { get; set; } = new DefaultCommon(); // Default implementation
    }
}
