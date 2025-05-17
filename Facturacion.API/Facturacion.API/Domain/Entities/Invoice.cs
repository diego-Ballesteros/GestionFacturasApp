using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;
namespace Facturacion.API.Domain.Entities;

[Table("Invoices")]
public class Invoice
{
    public Invoice() 
    {
        Details = new HashSet<InvoiceDetail>(); 
    }
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Customer name is required.")] 
    [StringLength(100, ErrorMessage = "Customer name cannot exceed 100 characters.")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Invoice date is required.")]
    public DateTime InvoiceDate { get; set; } 

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public virtual ICollection<InvoiceDetail> Details { get; set; }

}
