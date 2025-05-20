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
    this.currentInvoiceId = Number(this.route.snapshot.paramMap.get('id')); 

    if (this.currentInvoiceId) {
      this.isEditMode = true;
      this.pageTitle = `Edit Invoice #${this.currentInvoiceId}`;
      this.loadInvoiceForEdit(this.currentInvoiceId);
    } else {
      this.isEditMode = false;
      this.pageTitle = 'Create New Invoice';
      this.initForm();
      this.addInvoiceDetail(); 
    }
  }

  initForm(invoice?: Invoice): void { // Acepta una factura opcional para poblar el formulario
    this.invoiceForm = this.fb.group({
      customerName: [invoice?.customerName || '', [Validators.required, Validators.maxLength(100)]],
      invoiceDate: [invoice ? this.formatDateForInput(invoice.invoiceDate) : this.getTodayDateString(), [Validators.required]],
      details: this.fb.array(
        invoice?.details ? invoice.details.map(detail => this.createDetailGroup(detail)) : [],
        [Validators.required, Validators.minLength(1)]
      )
    });

    if (!this.isEditMode && this.details.length === 0) {
        this.addInvoiceDetail();
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
    this.invoiceService.getInvoiceById(id).subscribe({
      next: (invoiceData) => {
        if (invoiceData) {
          this.initForm(invoiceData);
        } else {
          this.errorMessage = `Invoice with ID ${id} not found.`;         
        }
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching invoice for edit:', err);
        this.errorMessage = 'Failed to load invoice data.';
        this.isLoading = false;
      }
    });
  }

  get details(): FormArray {
    return this.invoiceForm.get('details') as FormArray;
  }

  
  newInvoiceDetail(detail?: InvoiceDetailItem): FormGroup {
    return this.fb.group({
      // No necesitamos el 'id' del detalle en el formulario para este enfoque de "reemplazar"
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
      invoiceDate: new Date(formValue.invoiceDate), // Convertir string de fecha a objeto Date
      details: formValue.details.map((detail: any) => ({ // Mapear los detalles del formulario al DTO
        productName: detail.productName,
        quantity: Number(detail.quantity),
        unitPrice: Number(detail.unitPrice)
      }))
    };

    if (this.isEditMode && this.currentInvoiceId) {
      // MODO EDICIÓN
      this.invoiceService.updateInvoice(this.currentInvoiceId, payload).subscribe({
        next: (updatedInvoice) => {
          console.log('Invoice updated successfully!', updatedInvoice);
          this.isLoading = false;
          this.router.navigate(['/invoices']); // O al detalle: ['/invoices', updatedInvoice.id]
        },
        error: (err) => this.handleFormError(err, 'update')
      });
    } else {
      // MODO CREACIÓN
      this.invoiceService.createInvoice(payload).subscribe({
        next: (response) => {
          console.log('Invoice created successfully!', response);
          this.isLoading = false;
          this.router.navigate(['/invoices']);
        },
        error: (err) => this.handleFormError(err, 'create')
      });
    }    
}

  private handleFormError(err: any, action: 'create' | 'update'): void {
    console.error(`Error ${action}ing invoice:`, err);
    this.errorMessage = `Failed to ${action} invoice. Please try again.`;
    if (err.error && typeof err.error === 'object') {
        // Si el backend devuelve un objeto de error con una propiedad 'Message' o 'errors'
        if (err.error.ErrorMessage) {
            this.errorMessage = err.error.ErrorMessage;
        } else if (err.error.errors) { // Común para errores de validación de FluentValidation desde el backend
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