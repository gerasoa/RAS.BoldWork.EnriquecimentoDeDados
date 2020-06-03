using CargaSiscori;
using CargaSiscori.Models;
using Npgsql;
using RAS.BoldWork.EnriquecimentoDeDados.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RAS.BoldWork.EnriquecimentoDeDados
{
    public class Process
    {
        public static void Enriquecimento()
        {
            // selecionar os registros dos capitulos 90, 29 e 30
            List<ComentarioSiscori> comentariosSiscori = ObterCapitulosParaEnriquecimentoAnvisa("29");

            if (comentariosSiscori.Count > 0)
            {
                EnriquecimentoAnvisa(comentariosSiscori);
            }
        }        

        public static void EnriquecimentoAnvisa(List<ComentarioSiscori> comentarioSiscori)
        {
            var registroAnvisa = string.Empty;
            var NumeroProcesso = string.Empty;
            var listaRegistrosAnvisa = new List<string>();

            foreach (var item in comentarioSiscori)
            {
                LocalizarRegistroAnvisaPorPalavra(item.Comentario, listaRegistrosAnvisa);
                LocalizarRegistroAnvisaPorLetra(item.Comentario, listaRegistrosAnvisa);

                PesquisarListaDeRegistros(listaRegistrosAnvisa);

                if (listaRegistrosAnvisa.Count > 0)
                {
                    //verificar se listaRegistrosAnvisa.ToString() converse um array em uma strinf com virgula 
                    registroAnvisa = listaRegistrosAnvisa.ToString();
                    NumeroProcesso = "";
                }
                else
                {
                    registroAnvisa = "Não Identificado";
                    NumeroProcesso = "Não Identificado";
                }
            }       
        }

        private static List<string> PesquisarListaDeRegistros(List<string> possiveisRegistrosAnvisa)
        {
            var registroObtido = string.Empty;

            foreach (var itemTeste in possiveisRegistrosAnvisa)
            {
                registroObtido = ConsultaAnvisa(itemTeste);

                if (registroObtido == null)
                    possiveisRegistrosAnvisa.Remove(itemTeste);
            }

            return possiveisRegistrosAnvisa;
        }

        private static string ConsultaAnvisa(string registroParaValidar)
        {
            return registroParaValidar.FirstOrDefault().ToString();
        }

        private static List<string> LocalizarRegistroAnvisaPorPalavra(string comentario, List<string> listaRegistrosAnvisa)
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
                        listaRegistrosAnvisa.Add(a.ToString());
                    }
                }

                if (item.Length == 11)
                {
                    long a = 0;
                    if (long.TryParse(item, out a))
                    {
                        listaRegistrosAnvisa.Add(a.ToString());
                    }
                }
            }

            return listaRegistrosAnvisa;
        }

        private static List<string> LocalizarRegistroAnvisaPorLetra(string comentario, List<string> listaRegistrosAnvisa)
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
                    if (numero.Length == 11)
                    {
                        listaRegistrosAnvisa.Add(numero.ToString());
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

            string sql = string.Format(@"select  * from ""Siscori"" where substring(""NcmCodigo"", 1,2) = '{0}' limit 1000", capitulo);
            using var cmd = new NpgsqlCommand(sql, con);

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
