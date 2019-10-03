using Czeum.Abstractions.DTO;
using Czeum.DTO.Converters;
using Newtonsoft.Json;

namespace Czeum.DTO.Wrappers
{
    [JsonConverter(typeof(MoveDataWrapperConverter))]
    public class MoveDataWrapper : Wrapper<MoveData>
    {
    }
}