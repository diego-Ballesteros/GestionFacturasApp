import { Routes } from '@angular/router';
import { InvoiceListComponent } from './components/invoice-list/invoice-list.component'; 
import { InvoiceFormComponent } from './components/invoice-form/invoice-form.component';

// Define tus rutas aquí
export const routes: Routes = [
  // Ruta para listar facturas, será la ruta por defecto y la que se muestre en '/invoices'
  { path: '', redirectTo: 'invoices', pathMatch: 'full' }, // Redirige la raíz a /invoices
  { path: 'invoices', component: InvoiceListComponent },   // Cuando la URL sea /invoices, muestra InvoiceListComponent
  { path: 'invoices/new', component: InvoiceFormComponent },
  // { path: 'invoices/edit/:id', component: InvoiceFormComponent },
  // { path: 'invoices/:id', component: InvoiceDetailComponent } // Para ver detalles (necesitarás InvoiceDetailComponent)
];
