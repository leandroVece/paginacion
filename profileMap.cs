using AutoMapper;
using Paginacion;
using Paginacion.Models;
using PaginacionViewModel;

public class profileMap : Profile
{
    public profileMap()
    {
        CreateMap<Pokemon, PokemonViewModel>().ReverseMap();
    }
}