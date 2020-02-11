using System.ComponentModel.DataAnnotations;

namespace Shiliyuanma.Generator.SqlServer.Models
{
    public class SqlViewModel
    {
        [Required(ErrorMessage = "连接串不能为空")]
        public string ConnectionString { get; set; } = "Server=localhost;Database=test;User ID=sa;Password=123456;";

        public string TableNames { get; set; }

        public string NamespaceName { get; set; } = "Shiliyuanma.Test";

        public string DbContextName { get; set; } = "MyDbContext";

        public bool UseDataAnnotations { get; set; }

        public string ErrorMsg { get; set; }
    }
}