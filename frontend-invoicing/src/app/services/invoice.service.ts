import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'; 
import { Observable } from 'rxjs'; 
import { environment } from '../../environments/environment'; 

export interface InvoiceSummary {
  id: number;
  customerName: string;
  invoiceDate: Date; 
  totalAmount: number;
}

export interface CreateInvoiceDetailPayload {
    productName: string;
    quantity: number;
    unitPrice: number;
}

export interface CreateInvoicePayload {
    customerName: string;
    invoiceDate: Date; 
    details: CreateInvoiceDetailPayload[];
}

export interface CreateInvoiceResponse {
    invoiceId: number;
}

export interface InvoiceDetailItem {
    id: number;
    productName: string;
    quantity: number;
    unitPrice: number;
    subtotal: number;
}

export interface Invoice {
    id: number;
    customerName: string;
    invoiceDate: Date; // O string
    totalAmount: number;
    details: InvoiceDetailItem[];
}


@Injectable({
  providedIn: 'root' 
})
export class InvoiceService {
  private apiUrl = environment.apiUrl + '/invoices'; 
  
  constructor(private http: HttpClient) { }
 
  public  getAllInvoices(): Observable<InvoiceSummary[]> {
    return this.http.get<InvoiceSummary[]>(this.apiUrl);
  }
  
  getInvoiceById(id: number): Observable<Invoice> {
    return this.http.get<Invoice>(`${this.apiUrl}/${id}`);
  }

  createInvoice(invoiceData: CreateInvoicePayload): Observable<CreateInvoiceResponse> {
    return this.http.post<CreateInvoiceResponse>(this.apiUrl, invoiceData);
  }

  updateInvoice(id: number, invoiceData: CreateInvoicePayload): Observable<Invoice> { 
    return this.http.put<Invoice>(`${this.apiUrl}/${id}`, invoiceData);
  }

  deleteInvoice(id: number): Observable<void> { 
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}