using System.Collections.Generic;

namespace BluePrism.WordNavigator.Common
{
    public interface IFileManagementService
    {
        IEnumerable<string> ReadContent(string path);
        IAsyncEnumerable<string> ReadContentAsync(string path);
    }
}