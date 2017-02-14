using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HtmlTable.Tests.ViewModel
{
    public class UsuarioViewModel
    {

        [DisplayName("Id")]
        public Guid IdUsuario { get; set; } = Guid.NewGuid();

        [DisplayName("Nome")]
        public string NomeUsuario { get; set; }
        [DisplayName("Email")]
        [EmailAddress]
        public string EmailUsuario { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [DisplayName("Data de Nascimento")]
        public DateTime DataNascimentoUsuario { get; set; }

    }
}