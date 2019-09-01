using System.Collections.Generic;
using System.Threading.Tasks;

namespace Station {
    public interface IBasePlugin {
        string Name { get; }
        IEnumerable<PluginField> Fields { get; }

        Task<PluginResponse> ImportFileAsync(Dictionary<string, dynamic> fields);
    }
}
