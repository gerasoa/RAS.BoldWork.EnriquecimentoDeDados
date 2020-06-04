using CargaSiscori;
using CargaSiscori.Models;
using Newtonsoft.Json;
using Npgsql;
using RAS.BoldWork.EnriquecimentoDeDados.Models;
using RAS.BoldWork.EnriquecimentoDeDados.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace RAS.BoldWork.EnriquecimentoDeDados
{
    public class Process
    {
        public static void Enriquecimento()
        {
            // selecionar os registros dos capitulos 90, 29 e 30
            List<ComentarioSiscori> comentariosSiscori = ObterCapitulosParaEnriquecimentoAnvisa("99");

            if (comentariosSiscori.Count > 0)
            {
                EnriquecimentoAnvisa(comentariosSiscori);
            }
        }        

        public static void EnriquecimentoAnvisa(List<ComentarioSiscori> comentarioSiscori)
        {
            //var registroAnvisa = string.Empty;
            //var NumeroProcesso = string.Empty;
            var listaRegistrosAnvisa = new List<KeyValuePair<string, string>>();

            foreach (var item in comentarioSiscori)
            {
                LocalizarRegistroAnvisaPorPalavra(item.Comentario, listaRegistrosAnvisa);

                if (listaRegistrosAnvisa.Count == 0)
                 LocalizarRegistroAnvisaPorLetra(item.Comentario, listaRegistrosAnvisa);

                PesquisarListaDeRegistros(listaRegistrosAnvisa);

                if (listaRegistrosAnvisa.Count > 0)
                {
                    item.RegistroAnvisa = string.Join(",", listaRegistrosAnvisa).ToString();
                    item.NumeroProcesso = "";
                }
                else
                {
                    item.RegistroAnvisa = "Não Identificado";
                    item.NumeroProcesso = "Não Identificado";
                }
            }       
        }

        private static List<KeyValuePair<string, string>> PesquisarListaDeRegistros(List<KeyValuePair<string, string>> possiveisRegistrosAnvisa)
        {
            //var registroObtido = new Produto();

            for (int i = possiveisRegistrosAnvisa.Count -1; i >= 0; i--)
            {
                var registroObtido = ConsultaAnvisa(possiveisRegistrosAnvisa[i].Key);

                if (registroObtido.content.Count == 0)
                    possiveisRegistrosAnvisa.RemoveAll(x => x.Key.Equals(possiveisRegistrosAnvisa[i].Key));
            }

            return possiveisRegistrosAnvisa;
        }

        private static Produto ConsultaAnvisa(string numeroDoRegistro)
        {
            Produto produto = new Produto();

            string URL = String.Format("https://consultas.anvisa.gov.br/api/consulta/genericos?count=10&filter%5BnumeroRegistro%5D={0}&page=1", numeroDoRegistro);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "GET";

            request.Headers.Add("Accept", " application/json, text/plain, */*");
            request.Headers.Add("Accept-Encoding", " gzip, deflate, br");
            request.Headers.Add("Accept-Language", " pt-BR, pt; q=0.5");
            request.Headers.Add("Authorization", " Guest");
            request.Headers.Add("Cache-Control", " no-cache");
            request.Headers.Add("Connection", " Keep-Alive");
            request.Headers.Add("Cookie", " _TRAEFIK_BACKEND=http://10.0.2.115:8080; _pk_ses.42.210e=1; _pk_id.42.210e=08fd40a2cac3e390.1586706970.1.1586707046.1586706970.; FGTServer=2DE20D8040A1176F71792EB219E8DA9BCEDF996805D330F1AFAB13D5103423AE685570373EACB70B61CDD992CE85");
            request.Headers.Add("Host", " consultas.anvisa.gov.br");
            request.Headers.Add("If-Modified-Since", " Sat, 26 Jul 1997 05:00:00 GMT");
            request.Headers.Add("Pragma", " no-cache");
            request.Headers.Add("Referer", " https://consultas.anvisa.gov.br/");
            request.Headers.Add("User-Agent", " Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36 Edge/18.18363");

            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                responseReader.Close();

                produto = JsonConvert.DeserializeObject<Produto>(response);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocorreu um erro com o termo {0}: {1} ", numeroDoRegistro, e.Message);
                
            }

            return produto;
        }

        private static List<KeyValuePair<string, string>> LocalizarRegistroAnvisaPorPalavra(string comentario, List<KeyValuePair<string, string>> listaRegistrosAnvisa)
        {
            var arrayPalavras = comentario.Split(" ");            

            foreach (var palavra in arrayPalavras)
            {
                var item = palavra
                            .Replace(".", "")
                            .Replace("/", "")
                            .Replace("\"", "")
                            .Replace(",", "")
                            .Replace("-", "")
                            .Replace("_", "")
                            .Replace(":", "");

                if (item.ToUpper().Contains("ANVISA") && item.Length > 11)
                {
                    var palavraAnvisa = item.Substring(item.Length - 11);

                    long a = 0;
                    if(long.TryParse(palavraAnvisa, out a))
                    {
                        listaRegistrosAnvisa.Insert(0, new KeyValuePair<string, string>(a.ToString(),string.Empty));
                    }
                }

                if (item.Length == 11)
                {
                    long a = 0;
                    if (long.TryParse(item, out a))
                    {
                        listaRegistrosAnvisa.Insert(0, new KeyValuePair<string, string>(a.ToString(), string.Empty));
                    }
                }
            }

            return listaRegistrosAnvisa;
        }

        private static List<KeyValuePair<string, string>> LocalizarRegistroAnvisaPorLetra(string comentario, List<KeyValuePair<string, string>> listaRegistrosAnvisa)
        {
            var arrDescricaoProduto = comentario.ToArray();
            var numero = new StringBuilder();

            foreach (var item in arrDescricaoProduto)
            {
                int n = 0;
                if (int.TryParse(item.ToString(), out n))
                {
                    numero.Append(n);
                }
                else
                {
                    if (numero.Length >= 9 && numero.Length <=11)
                    {
                        if (!listaRegistrosAnvisa.Contains(new KeyValuePair<string, string>(numero.ToString(), string.Empty)))
                            listaRegistrosAnvisa.Insert(0, new KeyValuePair<string, string>(numero.ToString(), string.Empty));                       
                    }

                        numero.Clear();
                }
            }

            

            return listaRegistrosAnvisa;
        }

        private static List<ComentarioSiscori> ObterCapitulosParaEnriquecimentoAnvisa(string capitulo)
        {
            var cs = Configuration.GetConnectionString();

            List<ComentarioSiscori> comentarios = new List<ComentarioSiscori>();

            using var con = new NpgsqlConnection(cs);
            con.Open();

            string sql = string.Format(@"select  * from ""Siscori"" where substring(""NcmCodigo"", 1,2) = '{0}' limit 10", capitulo);
            using var cmd = new NpgsqlCommand(sql, con) { CommandTimeout = 120 } ;

            using NpgsqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                comentarios.Add(new ComentarioSiscori { 
                    Id = rdr.GetGuid(0),
                    Comentario = rdr.GetString(12).ToString()
                });
            }

            return comentarios;
        }
    }
}
