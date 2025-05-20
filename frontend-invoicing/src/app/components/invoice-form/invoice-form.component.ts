import { Component, OnInit, inject } from '@angular/core';
import { CommonModule, Location } from '@angular/common'; 
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router'; 
import { InvoiceService, CreateInvoicePayload, Invoice, InvoiceDetailItem } from '../../services/invoice.service'; 



@Component({
  selector: 'app-invoice-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule 
  ],
  templateUrl: './invoice-form.component.html',
  styleUrls: ['./invoice-form.component.scss']
})
export class InvoiceFormComponent implements OnInit {
  invoiceForm!: FormGroup; 
  isLoading: boolean = false;
  errorMessage: string | null = null;
  isEditMode: boolean = false;
  currentInvoiceId: number | null = null;
  pageTitle: string = 'Create New Invoice';

  private fb = inject(FormBuilder);
  private invoiceService = inject(InvoiceService);
  private router = inject(Router);
  private route = inject(ActivatedRoute); 
  private location = inject(Location);

  ngOnInit(): void {

    this.initForm();

    this.currentInvoiceId = Number(this.route.snapshot.paramMap.get('id')); 

    if (this.currentInvoiceId) {
      this.isEditMode = true;
      this.pageTitle = `Edit Invoice #${this.currentInvoiceId}`;
      this.loadInvoiceForEdit(this.currentInvoiceId);
    } else {
      this.isEditMode = false;
      this.pageTitle = 'Create New Invoice';
      if (this.details.length === 0) { 
        this.addInvoiceDetail();
      }
    }
  }

  initForm(invoice?: Invoice): void {
    this.invoiceForm = this.fb.group({
      customerName: ['', [Validators.required, Validators.maxLength(100)]],
      invoiceDate: [this.getTodayDateString(), [Validators.required]],
      details: this.fb.array(
        [], 
        [Validators.required, Validators.minLength(1)]
      )
    });

    if (invoice) {
      this.invoiceForm.patchValue({
        customerName: invoice.customerName,
        invoiceDate: this.formatDateForInput(invoice.invoiceDate)
      });
      this.details.clear();
      invoice.details.forEach(detail => {
        this.details.push(this.createDetailGroup(detail));
      });
    }
  }

  formatDateForInput(date: Date | string): string {
    const d = new Date(date);
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');
    return `<span class="math-inline">\{d\.getFullYear\(\)\}\-</span>{month}-${day}`;
  }

  getTodayDateString(): string {
    const today = new Date();
    const month = (today.getMonth() + 1).toString().padStart(2, '0');
    const day = today.getDate().toString().padStart(2, '0');
    return `<span class="math-inline">\{today\.getFullYear\(\)\}\-</span>{month}-${day}`;
  }

  loadInvoiceForEdit(id: number): void {
    this.isLoading = true; 
    this.errorMessage = null;
    this.invoiceService.getInvoiceById(id).subscribe({
      next: (invoiceData) => {
        if (invoiceData) {
          this.initForm(invoiceData); 
        } else {
          this.errorMessage = `Invoice with ID ${id} not found.`;
          this.pageTitle = 'Create New Invoice'; 
          this.isEditMode = false;
          this.currentInvoiceId = null;
          this.initForm();
          if (this.details.length === 0) this.addInvoiceDetail();
        }
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching invoice for edit:', err);
        this.errorMessage = 'Failed to load invoice data.';
        this.isLoading = false;
        this.initForm();
        if (this.details.length === 0) this.addInvoiceDetail();
      }
    });
  }

  get details(): FormArray {
    return this.invoiceForm.get('details') as FormArray;
  }

  
  newInvoiceDetail(detail?: InvoiceDetailItem): FormGroup {
    return this.fb.group({
      productName: [detail?.productName || '', [Validators.required, Validators.maxLength(100)]],
      quantity: [detail?.quantity || 1, [Validators.required, Validators.min(1)]],
      unitPrice: [detail?.unitPrice || 0.01, [Validators.required, Validators.min(0.01)]]
    });
  }

  createDetailGroup(detail: InvoiceDetailItem): FormGroup {
    return this.fb.group({
        productName: [detail.productName, [Validators.required, Validators.maxLength(100)]],
        quantity: [detail.quantity, [Validators.required, Validators.min(1)]],
        unitPrice: [detail.unitPrice, [Validators.required, Validators.min(0.01)]]
    });
  }

  addInvoiceDetail(): void {
    this.details.push(this.newInvoiceDetail());
  }

  removeInvoiceDetail(index: number): void {
    if (this.details.length > 1) { 
        this.details.removeAt(index);
    } else {       
        console.warn("Cannot remove the last detail item. At least one detail is required.");
    }
  }

  calculateGrandTotal(): number {
    let total = 0;
    this.details.controls.forEach(control => {
      const quantity = control.get('quantity')?.value || 0;
      const unitPrice = control.get('unitPrice')?.value || 0;
      total += quantity * unitPrice;
    });
    return total;
  }

  goBack(): void { 
    this.location.back(); 
  }

  onSubmit(): void {

    if (this.invoiceForm.invalid) {
      this.errorMessage = 'Please correct the form errors.';
      this.invoiceForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    const formValue = this.invoiceForm.value;

    const payload: CreateInvoicePayload = {
      customerName: formValue.customerName,
      invoiceDate: new Date(formValue.invoiceDate),
      details: formValue.details.map((detail: any) => ({ 
        productName: detail.productName,
        quantity: Number(detail.quantity),
        unitPrice: Number(detail.unitPrice)
      }))
    };

    if (this.isEditMode && this.currentInvoiceId) {
      // MODO EDICIÓN
      this.invoiceService.updateInvoice(this.currentInvoiceId, payload).subscribe({
        next: (updatedInvoice) => {
          this.isLoading = false;
          this.router.navigate(['/invoices']); 
        },
        error: (err) => {
          console.error('Error updating invoice from service:', err); 
          this.handleFormError(err, 'update');
        }
      });
    } else {
      // MODO CREACIÓN
      this.invoiceService.createInvoice(payload).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.router.navigate(['/invoices']);
        },
        error: (err) => {
          console.error('Error creating invoice:', err); 
          this.handleFormError(err, 'create');
        }
      });
    }    
}

  private handleFormError(err: any, action: 'create' | 'update'): void {
    console.error(`Error ${action}ing invoice:`, err);
    this.errorMessage = `Failed to ${action} invoice. Please try again.`;
    if (err.error && typeof err.error === 'object') {
        if (err.error.ErrorMessage) {
            this.errorMessage = err.error.ErrorMessage;
        } else if (err.error.errors) { 
            const validationErrors = err.error.errors;
            const firstErrorKey = Object.keys(validationErrors)[0];
            this.errorMessage = validationErrors[firstErrorKey]?.join(', ') || `Failed to ${action} invoice.`;
        }
    } else if (err.message) {
        this.errorMessage = err.message;
    }
    this.isLoading = false;
  }
}