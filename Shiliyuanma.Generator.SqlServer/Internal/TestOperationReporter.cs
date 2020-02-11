using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Design.Internal;

namespace Shiliyuanma.Generator.SqlServer.Internal
{
    internal class TestOperationReporter : IOperationReporter
    {
        public List<string> Messages { get; } = new List<string>();

        public void WriteError(string message) => Messages.Add("error: " + message);

        public void WriteWarning(string message) => Messages.Add("warn: " + message);

        public void WriteInformation(string message) => Messages.Add("info: " + message);

        public void WriteVerbose(string message) => Messages.Add("verbose: " + message);
    }
}