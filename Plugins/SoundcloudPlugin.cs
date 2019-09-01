using System.Collections.Generic;
using System.Threading.Tasks;

namespace Station {
    public class SoundcloudPlugin : IBasePlugin {
        public string Name { get; } = "Soundcloud";

        public IEnumerable<PluginField> Fields { get; } = new List<PluginField> {
            new PluginField {
                Label = "URL",
                Type = PluginField.FieldType.TextInput,
                Identifier = "url",
                IsRequired = true,
            }
        };

        public async Task<PluginResponse> ImportFileAsync(Dictionary<string, dynamic> fields) {
            return new PluginResponse {
                Data = new byte[] {},
                Album = "Night Time, My Time",
                Title = "You're Not The One",
                Artist = "Sky Ferreira",
                Duration = 180,
            };
        }
    }
}
