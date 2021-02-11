using BluePrism.WordNavigator.Common.Services;
using System.Collections.Generic;

namespace BluePrism.WordNavigator.Common
{
    public interface IFileManagementService : IService
    {
        IEnumerable<string> ReadContent(string path);
        IAsyncEnumerable<string> ReadContentAsync(string path);
    }
}