using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.Config
{
    public class AzureBlobSettingsConfig
    {
        public string ProfilePicContainerName { get; set; } = null!;
        public string FileContainerName { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
    }
}
