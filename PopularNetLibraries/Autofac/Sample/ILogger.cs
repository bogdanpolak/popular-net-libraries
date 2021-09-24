namespace PopularNetLibraries.Autofac.Sample
{
    public interface ILogger
    {
        public void Warning(string message);
        public void Info(string message);
        int GetMessagesCount();
    }
}
