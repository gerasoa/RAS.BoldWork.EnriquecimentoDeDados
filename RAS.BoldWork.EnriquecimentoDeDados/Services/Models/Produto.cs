using System;
using System.Collections.Generic;
using System.Text;

namespace RAS.BoldWork.EnriquecimentoDeDados.Services.Models
{
    public class Produto
    {
        public string last { get; set; }
        public string totalElements { get; set; }
        public string totalPages { get; set; }
        public string first { get; set; }
        public string numberOfElements { get; set; }
        public string sort { get; set; }
        public string size { get; set; }
        public string number { get; set; }
        public IList<Content> content { get; set; }
    }

    public class Content
    {
        public string nomeProduto { get; set; }
        public string processo { get; set; }
        public string registro { get; set; }
        public string razaoSocial { get; set; }
        public string cnpj { get; set; }
        public string situacao { get; set; }
        public string dataVencimento { get; set; }
        public string codigoTipo { get; set; }
        public string descSituacao { get; set; }
        public string descTipo { get; set; }
    }
}
