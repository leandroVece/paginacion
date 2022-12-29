
using System.Net;
using System.Text.Json;
using static Paginacion.poke;

namespace Paginacion;

class api
{
    public HttpWebRequest reqs(string url)
    {
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "application/json";
        request.Accept = "application/json";

        return request;
    }

    public Root GetAll()
    {
        var request = reqs("https://pokeapi.co/api/v2/pokemon");
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
                        Root List = JsonSerializer.Deserialize<Root>(responseBody);
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
}