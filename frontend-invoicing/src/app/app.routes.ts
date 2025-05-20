import { Routes } from '@angular/router';
import { InvoiceListComponent } from './components/invoice-list/invoice-list.component'; 
import { InvoiceFormComponent } from './components/invoice-form/invoice-form.component';

export const routes: Routes = [
  { path: '', redirectTo: 'invoices', pathMatch: 'full' }, 
  { path: 'invoices', component: InvoiceListComponent },  
  { path: 'invoices/new', component: InvoiceFormComponent },
  { path: 'invoices/edit/:id', component: InvoiceFormComponent },
  // { path: 'invoices/:id', component: InvoiceDetailComponent } // Para ver detalles (necesitarás InvoiceDetailComponent)
];
