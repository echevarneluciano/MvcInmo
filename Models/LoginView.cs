using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcInmo.Models;

public class LoginView
{
    [DataType(DataType.EmailAddress)]
    public string Usuario { get; set; }
    [DataType(DataType.Password)]
    public string Clave { get; set; }
}
