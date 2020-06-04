using System;
using System.Collections.Generic;
using System.Text;

namespace RAS.BoldWork.EnriquecimentoDeDados.Services.Models
{

    public class EmpresaDetail
    {
        //public Guid SiscoriId { get; set; }
        public string produto { get; set; }
        public Empresa Empresa { get; set; }
        public Mensagem mensagem { get; set; }
        public string nomeTecnico { get; set; }
        public string registro { get; set; }
        public bool cancelado { get; set; }
        public string dataCancelamento { get; set; }
        public string processo { get; set; }
        public List<Apresentaco> apresentacoes { get; set; }
        public List<Fabricante> fabricantes { get; set; }
        public Risco risco { get; set; }
        public Vencimento vencimento { get; set; }
        public string publicacao { get; set; }
        public bool apresentacaoModelo { get; set; }
        public List<Arquivo> arquivos { get; set; }
        public string processoMedidaCautelar { get; set; }
        public string tooltip { get; set; }
    }

    public class Empresa
    {
        public Guid EmpresaDetailId { get; set; }
        public EmpresaDetail EmpresaDetail { get; set; }
        public string cnpj { get; set; }
        public string razaoSocial { get; set; }
        public string autorizacao { get; set; }
    }

    public class Mensagem
    {
        public Guid EmpresaDetailId { get; set; }
        public EmpresaDetail EmpresaDetail { get; set; }
        public string situacao { get; set; }
        public string resolucao { get; set; }
        public string motivo { get; set; }
        public bool negativo { get; set; }
    }

    public class Apresentaco
    {
        public Guid EmpresaDetailId { get; set; }
        public EmpresaDetail EmpresaDetail { get; set; }
        public string modelo { get; set; }
        public string componente { get; set; }
        public string apresentacao { get; set; }
    }

    public class Fabricante
    {
        public Guid EmpresaDetailId { get; set; }
        public EmpresaDetail EmpresaDetail { get; set; }
        public string atividade { get; set; }
        public string razaoSocial { get; set; }
        public string pais { get; set; }
        public string local { get; set; }
    }

    public class Risco
    {
        public Guid EmpresaDetailId { get; set; }
        public EmpresaDetail EmpresaDetail { get; set; }
        public string sigla { get; set; }
        public string descricao { get; set; }
    }

    public class Vencimento
    {
        public Guid EmpresaDetailId { get; set; }
        public EmpresaDetail EmpresaDetail { get; set; }
        public DateTime? data { get; set; }
        public string descricao { get; set; }
    }

    public class Arquivo
    {
        public Guid EmpresaDetailId { get; set; }
        public EmpresaDetail EmpresaDetail { get; set; }
        public string Nome { get; set; }
        public string Url { get; set; }
    }
}
