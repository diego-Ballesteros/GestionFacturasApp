import { Component, OnInit, inject } from '@angular/core';
import { CommonModule, Location } from '@angular/common'; 
import { ActivatedRoute, Router, RouterModule } from '@angular/router'; 
import { InvoiceService, Invoice, InvoiceDetailItem } from '../../services/invoice.service'; 
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators'; 

@Component({
  selector: 'app-invoice-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule 
  ],
  templateUrl: './invoice-detail.component.html',
  styleUrls: ['./invoice-detail.component.scss']
})
export class InvoiceDetailComponent implements OnInit {
  invoice: Invoice | null = null; 
  isLoading: boolean = true;
  errorMessage: string | null = null;

  private route = inject(ActivatedRoute);
  private invoiceService = inject(InvoiceService);
  private router = inject(Router); 
  private location = inject(Location); 

  ngOnInit(): void {
    this.loadInvoiceDetails();
  }

  loadInvoiceDetails(): void {
    this.isLoading = true;
    this.errorMessage = null;

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const invoiceId = Number(idParam);
      if (!isNaN(invoiceId)) {
        this.invoiceService.getInvoiceById(invoiceId).subscribe({
          next: (data) => {
            if (data) {
              this.invoice = data;
            } else {
              this.errorMessage = `Invoice with ID ${invoiceId} not found.`;
            }
            this.isLoading = false;
          },
          error: (err) => {
            console.error('Error fetching invoice details:', err);
            this.errorMessage = 'Failed to load invoice details.';
            if (err.message && err.message.includes('self-signed certificate')) {
                this.errorMessage = 'Failed to load invoice due to a certificate issue. Please ensure the backend SSL certificate is trusted.';
            }
            this.isLoading = false;
          }
        });
      } else {
        this.errorMessage = 'Invalid invoice ID in URL.';
        this.isLoading = false;
      }
    } else {
      this.errorMessage = 'No invoice ID provided in URL.';
      this.isLoading = false;
    }
  }

  goBack(): void {
    this.location.back();
  }

  navigateToEdit(): void {
    if (this.invoice) {
      this.router.navigate(['/invoices/edit', this.invoice.id]);
    }
  }
}
