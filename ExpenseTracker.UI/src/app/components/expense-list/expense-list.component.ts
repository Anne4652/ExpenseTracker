import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Expense } from '../../models/expense';
import { ExpenseService } from '../../services/expense.service';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-expense-list',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule, CommonModule, HttpClientModule],
  templateUrl: './expense-list.component.html',
  styleUrl: './expense-list.component.css'
})
export class ExpenseListComponent{
  expenses: Expense[] = [];

  constructor(private expenseService: ExpenseService) {}

  ngOnInit(): void {
    this.expenseService.getAllExpenses().subscribe(data => {
      this.expenses = data;
    });
  }
}
