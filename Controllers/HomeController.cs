using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Paginacion.Models;
using AutoMapper;
using PaginacionViewModel;
using X.PagedList;

namespace Paginacion.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMapper map;

    public HomeController(ILogger<HomeController> logger, IMapper mapper)
    {
        _logger = logger;
        map = mapper;
    }

    public IActionResult Index(int? pageSize, int? page)
    {
        api list = new api();
        poke a = new poke(list.GetAll());


        List<Pokemon> dates = new List<Pokemon>();
        foreach (var item in a.Pokemon.results)
        {
            dates.Add(list.GetOne(item.url)); ;
        }
        var listpoke = map.Map<List<PokemonViewModel>>(dates);

        pageSize = (pageSize ?? 4);
        page = (page ?? 1);
        ViewBag.pageSize = pageSize;

        var pasar = listpoke.ToPagedList(page.Value, pageSize.Value);

        return View(pasar);
    }

    public IActionResult Privacy()
    {
        api list = new api();
        poke a = new poke(list.GetAll());


        List<Pokemon> dates = new List<Pokemon>();
        foreach (var item in a.Pokemon.results)
        {
            dates.Add(list.GetOne(item.url)); ;
        }
        var listpoke = map.Map<List<PokemonViewModel>>(dates);
        return View(listpoke);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }




}
