using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.Config
{
    public class AzureBlobSettingsConfig
    {
        public string ContainerName { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
    }
}
