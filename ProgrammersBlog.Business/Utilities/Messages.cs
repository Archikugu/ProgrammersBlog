namespace ProgrammersBlog.Business.Utilities;

public static class Messages
{
    // Messages for Category
    public static class Category
    {
        public static string NotFound(bool isPlural)
        {
            return isPlural ? "Such categories could not be found." : "Such a category could not be found.";
        }
        public static string Add(string categoryName)
        {
            return $"{categoryName} category has been successfully added.";
        }
        public static string Delete(string categoryName)
        {
            return $"{categoryName} category has been successfully deleted.";
        }
        public static string HardDelete(string categoryName)
        {
            return $"{categoryName} category has been permanently deleted from the database.";
        }
        public static string Update(string categoryName)
        {
            return $"{categoryName} category has been successfully updated.";
        }
    }

    // Messages for Article

    public static class Article
    {
        public static string NotFound(bool isPlural)
        {
            return isPlural ? "Such articles could not be found." : "Such an article could not be found.";
        }
        public static string Add(string articleName)
        {
            return $"{articleName} article has been successfully added.";
        }
        public static string Delete(string articleName)
        {
            return $"{articleName} article has been successfully deleted.";
        }
        public static string HardDelete(string articleName)
        {
            return $"{articleName} article has been permanently deleted from the database.";
        }
        public static string Update(string articleName)
        {
            return $"{articleName} article has been successfully updated.";
        }
    }
}
