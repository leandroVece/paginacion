# Paginacion con Emtity Framework

Para este proyecto utilice ASP.Net Core 6.0 y Visual Studio Code.
Crear un nuevo proyecto MVC con el siguiente comando.

```asp
dotnet new mvc
```
Emtity Framework nos brinda herramientas muy ultilez que nos facilita el trabajo. Para iniciar este proyecto es necesario intalar las dependicia que dejare a continuacion.

    dotnet add package X.PagedList --version 8.4.3
	dotnet add package X.PagedList.Mvc --version 8.0.7
	dotnet add package X.PagedList.Mvc.Core --version 8.4.3

Para verciones superiores a la 4.8 es recomendable usar esta vercion. para verciones inferiores Nuget ofrece otra vercion que dejare a continuacion.

    dotnet add package PagedList --version 1.17.0
    dotnet add package PagedList.Mvc --version 4.5.0

Una vez instalado las dependencias podemos inicaiar con el proyecto. Para ello remplace la base de datos con una api de [Pokemon]("https://pokeapi.co/api/v2/pokemon" "Pokemon").
Para mantenerlo simple, por fines puramente practicos, cree una nueva clase dentro de la carpeta Models llamada api en el cual deje los metodos para conectarme con ella.

    public HttpWebRequest reqs(string url)
    {
    	var request = (HttpWebRequest)WebRequest.Create(url);
    	request.Method = "GET";
    	request.ContentType = "application/json";
    	request.Accept = "application/json";
    
    	return request;
    }
Nota: todas las dependencias faltantes usen el intellisense de C#
Como no es el tema hablar sobre el consumir una Api, hablare poco sobre el tema. Con esta funcion obtengo la respusta de la api que me traera los datos que Utilizaremos para este ejemplo.
con la funcion GetAll() deseriallizo el archivo json en un objeto consumible por C#. para esto hice uso de una pagina [json2csharp](https://json2csharp.com/ "json2csharp") que me facilito el modelo que el objeto "Pokemon" tendria.
Por conveniencia guarde este resultado en la clase poke donde inicialice la clase Root en el contructor del mismo.

    class poke
    {
        public Root Pokemon { get; set; }
        public poke(Root pokemon)
        {
            Pokemon = pokemon;
        }
    }
    
    public class Result
    {
        public string name { get; set; }
        public string url { get; set; }
    }
    
    public class Root
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<Result> results { get; set; }
    }

Esta clase en el constructor Home en el metodo "Index" para obtener los polemones de la api. Por defecto esta api trae los 20 primeros pokemones, que seran suficiente para mostrar este ejemplo.
Como se puede ver, el objeto Poke no tiene mucha informacion sobre los pokemones. eso es debido a que dicha informacion es necesaria obtenerla a travez de la url.
para ello Use el metodo GetOne() para optener la informacion que queria de cada pokemon.

	public Pokemon GetOne(string url)
    {
        var request = reqs(url);

        try
        {
            using (WebResponse response = request.GetResponse())
            {
                using (Stream strReader = response.GetResponseStream())
                {
                    if (strReader == null) return null;
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        string responseBody = objReader.ReadToEnd();
                        Pokemon List = JsonSerializer.Deserialize<Pokemon>(responseBody);
                        return List;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

Para capturar el objeto use [json2csharp](https://json2csharp.com/ "json2csharp") para obtener las clases del objeto pokemon que guarde en el archivo pokeDate.cs
En el Metodo Index instanciaremos estas clases para poder obtener la informacion que se presentara en la vista.

     	api list = new api();
    	poke a = new poke(list.GetAll());
    
    	List<Pokemon> dates = new List<Pokemon>();
    	foreach (var item in a.Pokemon.results)
    	{
    	dates.Add(list.GetOne(item.url)); ;
    	}
    	var listpoke = map.Map<List<PokemonViewModel>>(dates);

Para no manejar informacion inecesaria hice un ViewModel llamado PokeViewModel que deje en la carpeta Models. En el solo guardo la informacion de Id, Namem, Order y Sprites.

    public class PokemonViewModel
    {
        public int id { get; set; }
        public bool is_default { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        public Sprites Sprites { get; set; }
    }

Para esto es necesario instalar dos depencias mas.

    dotnet add package AutoMapper --version 12.0.0
    dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.0

Una vez instaladas nos vamos al archivo Program.cs y pegamos la siguiente linea de codigo

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

Nota: Tiene que estar antes de la linea.

    var app = builder.Build();

Despues en un archivo que llame profileMap aparte crearemos la conversion entre las dos clases.

	public class profileMap : Profile
	{
		public profileMap()
		{
			CreateMap<Pokemon, PokemonViewModel>().ReverseMap();
		}
	}

Con esto podremos obtener Nombre de los pokemones y sus imagenes. Con todo esto ahora podemoos centrarnos en la Paginacion
En el metodo Index iniciaremos tres variables que nos permitiran saber el index de la paginacion y la cantidad de elementos que tendra cada pagina.

    	pageSize = (pageSize ?? 4);
    	page = (page ?? 1);
    	ViewBag.pageSize = pageSize;
    '''en este momento se me acabo la imaginacion para los nombre'''
    	var pasar = listpoke.ToPagedList(page.Value, pageSize.Value);
		return View(pasar);

Luego convertimos la Lista en una PageList con el metodo ToPagedList que recibira el valor de la pagina y su tamaño;
pasando esta lista como parametro en la vista solo nos queda llamar a las dependencias y trabajar la logica para mostrar los archivos.
Antes de eso tendremos que instalar la ultima dependencia para el estilo de la paginacion. ya que en visual estudio code, no se descarga el archivo PagedList.css que da el estilo que conocemos

    	dotnet add package X.PagedList.Mvc.Bootstrap4 --version 8.1.0

Ahora solo queda los toque finales para presentar la vista.

    @model X.PagedList.IPagedList<PaginacionViewModel.PokemonViewModel>;
    @using X.PagedList
    @using X.PagedList.Mvc.Bootstrap4.Core
    @using X.PagedList.Mvc.Core
    @using X.PagedList.Web.Common
    
    
    @{
        ViewData["Title"] = "Home Page";
    
    }
    
    <div class="text-center">
        <h1 class="display-4">Welcome</h1>
        <p>Learn about <a href="https://docs.microsoft.com/aspnet/core">building Web apps with ASP.NET Core</a>.</p>
    </div>
    
    
    
    <div class="d-flex align-content-center flex-wrap ">
        @foreach (var item in Model)
        {
            <div class="card  m-1 " style="width: 120px;">
                <img src="@item.Sprites.front_default" class="card-img-top" alt="...">
                <div class="card-body">
                    <p class="card-text">@item.name</p>
                </div>
    
            </div>
        }
    </div>
    
    <br>
    page @( Model.PageCount < Model.PageNumber ? 0 : Model.PageNumber) of @Model.PageCount
    <br>
    Total Iten Count @Model.TotalItemCount
    
    
    @Html.PagedListPager(Model, Page => Url.Action("Index", new {Page, pageSize = ViewBag.pageSize}),
    Bootstrap4PagedListRenderOptions.ClassicPlusFirstAndLast)
