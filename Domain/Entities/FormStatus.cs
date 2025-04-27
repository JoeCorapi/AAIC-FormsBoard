using System.ComponentModel.DataAnnotations.Schema;

namespace FormsBoard.Domain.Entities
{
    [Table("FormStatus")]
    public class FormStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
