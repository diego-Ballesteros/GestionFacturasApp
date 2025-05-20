import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { RouterModule } from '@angular/router'; 
import { InvoiceService, InvoiceSummary } from '../../services/invoice.service'; 
import { Router } from '@angular/router';

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

  constructor(
    private invoiceService: InvoiceService,
    private router: Router
  ) { }

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
      },
      error: (err) => {
        console.error('Error al cargar facturas en el componente:', err); 
        this.errorMessage = 'Failed to load invoices. Please check connection or try again later.';
        if (err.message && err.message.includes('self-signed certificate')) {
          this.errorMessage = 'Failed to load invoices due to a certificate issue. Please ensure the backend SSL certificate is trusted.';
        }
        this.isLoading = false;
      }
    });
  }

  viewInvoice(id: number): void {
    console.log('View invoice:', id);
  }

  editInvoice(id: number): void {
    console.log('Edit invoice:', id);
    this.router.navigate(['/invoices/edit', id]);
  }

  deleteInvoice(id: number): void {
    console.log('Attempting to delete invoice:', id);
    if (confirm(`Are you sure you want to delete invoice ID ${id}?`)) {
      this.invoiceService.deleteInvoice(id).subscribe({
        next: () => {
          console.log(`Invoice ${id} deleted successfully`);
          this.loadInvoices(); 
        },
        error: (err) => {
          console.error(`Error deleting invoice ${id}:`, err);
          this.errorMessage = `Failed to delete invoice ID ${id}.`;
        }
      });
    }
  }

  navigateToCreate(): void {
    console.log('Navigate to create invoice');
    this.router.navigate(['/invoices/new']);
  }
}
