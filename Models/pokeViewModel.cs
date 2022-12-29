
using System.Net;
using System.Text.Json;
using Paginacion;

namespace PaginacionViewModel
{
    public class PokemonViewModel
    {
        public int id { get; set; }
        public bool is_default { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        public Sprites Sprites { get; set; }

    }

}