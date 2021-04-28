using System.Collections.Generic;

namespace PopularNetLibraries.Autofac.Sample
{
    public class FakeLogger : ILogger
    {
        private List<string> Messages { get; init; } = new List<string>();

        public int GetMessagesCount() => Messages.Count;

        public void Info(string message)
        {
            Messages.Add($"INFO: {message}");
        }

        public void Warning(string message)
        {
            Messages.Add($"WARNING: {message}");
        }
    }
}
