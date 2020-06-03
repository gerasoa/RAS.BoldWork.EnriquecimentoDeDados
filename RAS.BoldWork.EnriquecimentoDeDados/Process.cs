using CargaSiscori;
using CargaSiscori.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAS.BoldWork.EnriquecimentoDeDados
{
    public class Process
    {
        public static void Enriquecimento()
        {
            // selecionar os registros dos capitulos 90, 29 e 30
            ObterCapitulosParaEnriquecimentoAnvisa();

            if (true)
            {
                EnriquecimentoAnvisa();
            }
        }

        private static IList<Siscori> ObterCapitulosParaEnriquecimentoAnvisa()
        {
            throw new NotImplementedException();
        }

        public static void EnriquecimentoAnvisa()
        {
            
        }
    }
}
