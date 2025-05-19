import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; // Para *ngIf, *ngFor, pipes (date, currency)
import { RouterModule } from '@angular/router'; // Para usar routerLink en el futuro
import { InvoiceService, InvoiceSummary } from '../../services/invoice.service'; // Ajusta la ruta si es necesario

@Component({
  selector: 'app-invoice-list',
  standalone: true,
  imports: [
    CommonModule, 
    RouterModule  
  ],
  templateUrl: './invoice-list.component.html',
  styleUrls: ['./invoice-list.component.scss']
})
export class InvoiceListComponent implements OnInit {
  invoices: InvoiceSummary[] = [];
  isLoading: boolean = true;
  errorMessage: string | null = null;

  constructor(private invoiceService: InvoiceService) { }

  ngOnInit(): void {
    this.loadInvoices();
  }

  loadInvoices(): void {
    this.isLoading = true;
    this.errorMessage = null;
    this.invoiceService.getAllInvoices().subscribe({
      next: (data) => {
        this.invoices = data;
        this.isLoading = false;
        console.log('Facturas cargadas en el componente:', this.invoices); 
      },
      error: (err) => {
        console.error('Error al cargar facturas en el componente:', err); // Para depuración
        // El error HttpErrorResponse con 'self-signed certificate' debería aparecer aquí si no se ha resuelto
        this.errorMessage = 'Failed to load invoices. Please check connection or try again later.';
        if (err.message && err.message.includes('self-signed certificate')) {
          this.errorMessage = 'Failed to load invoices due to a certificate issue. Please ensure the backend SSL certificate is trusted.';
        }
        this.isLoading = false;
      }
    });
  }

  // Métodos para los botones de acción (los conectaremos/implementaremos más tarde)
  viewInvoice(id: number): void {
    console.log('View invoice:', id);
    // Navegar a la vista de detalle: this.router.navigate(['/invoices', id]);
  }

  editInvoice(id: number): void {
    console.log('Edit invoice:', id);
    // Navegar al formulario de edición: this.router.navigate(['/invoices/edit', id]);
  }

  deleteInvoice(id: number): void {
    console.log('Attempting to delete invoice:', id);
    if (confirm(`Are you sure you want to delete invoice ID ${id}?`)) {
      this.invoiceService.deleteInvoice(id).subscribe({
        next: () => {
          console.log(`Invoice ${id} deleted successfully`);
          this.loadInvoices(); // Recargar la lista después de eliminar
        },
        error: (err) => {
          console.error(`Error deleting invoice ${id}:`, err);
          this.errorMessage = `Failed to delete invoice ID ${id}.`;
          // Aquí también podrías tener un error de certificado si es la primera interacción HTTPS después de un problema
        }
      });
    }
  }

  // Para el botón "Create New Invoice" (lo conectaremos con el enrutador)
  navigateToCreate(): void {
    console.log('Navigate to create invoice');
    // this.router.navigate(['/invoices/new']);
  }
}
