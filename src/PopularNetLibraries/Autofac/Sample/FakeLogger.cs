using System.Collections.Generic;

namespace PopularNetLibraries.Autofac.Sample
{
    public class FakeLogger : ILogger
    {
        private readonly List<string> _messages = new List<string>();

        public int GetMessagesCount() => _messages.Count;

        public void Info(string message)
        {
            _messages.Add($"INFO: {message}");
        }

        public void Warning(string message)
        {
            _messages.Add($"WARNING: {message}");
        }
    }
}
