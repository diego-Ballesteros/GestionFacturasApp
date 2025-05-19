import { Component, OnInit, inject } from '@angular/core'; // Importa inject
import { CommonModule } from '@angular/common'; // Para *ngIf, *ngFor, etc.
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms'; // Para Reactive Forms
import { Router } from '@angular/router'; // Para navegar después de crear
import { InvoiceService, CreateInvoicePayload, CreateInvoiceDetailPayload } from '../../services/invoice.service'; // Tu servicio e interfaces

@Component({
  selector: 'app-invoice-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule // <--- IMPORTANTE para [formGroup], formControlName, etc.
  ],
  templateUrl: './invoice-form.component.html',
  styleUrls: ['./invoice-form.component.scss']
})
export class InvoiceFormComponent implements OnInit {
  invoiceForm!: FormGroup; // '!' indica que se inicializará en ngOnInit o el constructor
  isLoading: boolean = false;
  errorMessage: string | null = null;

  // Inyección de dependencias moderna con inject() o a través del constructor
  private fb = inject(FormBuilder);
  private invoiceService = inject(InvoiceService);
  private router = inject(Router);
  // Alternativa con constructor:
  // constructor(
  //   private fb: FormBuilder,
  //   private invoiceService: InvoiceService,
  //   private router: Router
  // ) {}

  ngOnInit(): void {
    this.initForm();
  }

  initForm(): void {
    this.invoiceForm = this.fb.group({
      customerName: ['', [Validators.required, Validators.maxLength(100)]],
      invoiceDate: [this.getTodayDateString(), [Validators.required]], // Fecha de hoy por defecto
      details: this.fb.array([], [Validators.required, Validators.minLength(1)]) // FormArray para los detalles, al menos uno es requerido
    });
    // Añadir al menos un detalle por defecto al crear el formulario
    this.addInvoiceDetail();
  }

  // Helper para obtener la fecha de hoy en formato YYYY-MM-DD para el input date
  getTodayDateString(): string {
    const today = new Date();
    const month = (today.getMonth() + 1).toString().padStart(2, '0');
    const day = today.getDate().toString().padStart(2, '0');
    return `<span class="math-inline">\{today\.getFullYear\(\)\}\-</span>{month}-${day}`;
  }

  // Getter para acceder fácilmente al FormArray de detalles en la plantilla
  get details(): FormArray {
    return this.invoiceForm.get('details') as FormArray;
  }

  // Método para crear un FormGroup para un nuevo detalle de factura
  newInvoiceDetail(): FormGroup {
    return this.fb.group({
      productName: ['', [Validators.required, Validators.maxLength(100)]],
      quantity: [1, [Validators.required, Validators.min(1)]],
      unitPrice: [0.01, [Validators.required, Validators.min(0.01)]]
      // No necesitamos Subtotal aquí, se calculará al enviar o en el backend
    });
  }

  // Método para añadir un nuevo detalle al FormArray
  addInvoiceDetail(): void {
    this.details.push(this.newInvoiceDetail());
  }

  // Método para eliminar un detalle del FormArray por su índice
  removeInvoiceDetail(index: number): void {
    if (this.details.length > 1) { // No permitir eliminar si solo queda uno (por Validators.minLength(1))
        this.details.removeAt(index);
    } else {
        // Podrías mostrar un mensaje al usuario
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

  goBackToList(): void {
    this.router.navigate(['/invoices']); // Navega de regreso a la lista de facturas
  }

  onSubmit(): void {
    if (this.invoiceForm.invalid) {
      this.errorMessage = 'Please correct the form errors.';
      // Marcar todos los campos como "touched" para que se muestren los errores de validación
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

    this.invoiceService.createInvoice(payload).subscribe({
      next: (response) => {
        console.log('Invoice created successfully!', response);
        this.isLoading = false;
        // Navegar a la lista de facturas o a la vista de detalle de la nueva factura
        this.router.navigate(['/invoices']); // Redirige a la lista
        // Opcional: this.router.navigate(['/invoices', response.invoiceId]); // Redirige al detalle
      },
      error: (err) => {
        console.error('Error creating invoice:', err);
        this.errorMessage = 'Failed to create invoice. Please try again.';
        // Podrías intentar parsear el error del backend si viene en un formato específico
        if (err.error && err.error.ErrorMessage) {
            this.errorMessage = err.error.ErrorMessage;
        } else if (err.message) {
            this.errorMessage = err.message;
        }
        this.isLoading = false;
      }
    });
  }
}
