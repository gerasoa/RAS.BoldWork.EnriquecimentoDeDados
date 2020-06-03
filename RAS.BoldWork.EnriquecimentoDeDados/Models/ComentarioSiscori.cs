using System;
using System.Collections.Generic;
using System.Text;

namespace RAS.BoldWork.EnriquecimentoDeDados.Models
{
    public class ComentarioSiscori
    {
        public Guid Id { get; set; }
        public string Comentario { get; set; }
        public string RegistroAnvisa { get; set; }
        public string NumeroProcesso { get; set; }
    }
}
