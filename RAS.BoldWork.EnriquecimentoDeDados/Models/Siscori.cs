using System;
using System.Collections.Generic;
using System.Text;

namespace CargaSiscori.Models
{
    public class Siscori
    {
        public Guid Id { get; set; }
        public string Ordem { get; set; }

        public string AnoMes { get; set; }

        public string NcmCodigo { get; set; }

        public string NcmDescricao { get; set; }

        public string PaisOrigemCodigo { get; set; }

        public string PaisDeOrigem { get; set; }

        public string PaisAquisicaoCodigo { get; set; }

       public string PaisDeAquisicao { get; set; }

        public string UnidEstat { get; set; }

        public string UnidadeDeMedida { get; set; }

        public string UnidadeComercial { get; set; }

        public string DescricaoDoProduto { get; set; }

        public string QuantidadeEstatistica { get; set; }

        public string PesoLiquido { get; set; }

        public string VolumeDolar { get; set; }

        public string VoumeFrenteDolar { get; set; }

        public string ValorSeguroDolar { get; set; }

        public string ValorUnidadeProdutoDolar { get; set; }

        public string QuantidadeComercial { get; set; }

        public string TotalunidadeProdutoDolar { get; set; }

        public string UnidadeDesembarque { get; set; }

        public string UnidadeDesembaraco { get; set; }

        public string Incoterm { get; set; }

        public string NatInformacao { get; set; }

        public string SituacaoDoDespacho { get; set; }
    }
}
