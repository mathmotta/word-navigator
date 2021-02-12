using BluePrism.WordNavigator.Common.Services.IO;
using Microsoft.Extensions.Logging;

namespace BluePrism.WordNavigator.Core.Navigation
{
    public class FileNavigationService : OnDemandFileService, IFileNavigationService
    {
        public FileNavigationService(ILogger<FileNavigationService> log) : base(log)
        {
        }
    }
}
