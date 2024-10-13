import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ExpenseService } from '../../services/expense.service';
import { CategoryService } from '../../services/category.service';  // Import CategoryService
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-expense-form',
  standalone: true,
  imports: [ReactiveFormsModule, HttpClientModule, CommonModule],
  templateUrl: './expense-form.component.html',
  styleUrls: ['./expense-form.component.css']
})
export class ExpenseFormComponent implements OnInit {
  expenseForm: FormGroup;
  isEditMode = false;
  expenseId: number | null = null;
  categories: any[] = [];

  constructor(
    private fb: FormBuilder,
    private expenseService: ExpenseService,
    private categoryService: CategoryService,  // Inject CategoryService here
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.expenseForm = this.fb.group({
      amount: [0, Validators.required],
      description: ['', Validators.required],
      date: ['', Validators.required],
      categoryId: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadCategories();

    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.isEditMode = true;
        this.expenseId = +id;
        this.expenseService.getExpenseById(this.expenseId).subscribe(data => {
          this.expenseForm.patchValue(data);
        });
      }
    });
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe((data) => {
      console.log('Fetched categories:', data);
      this.categories = data;
    }, error => {
      console.error('Error fetching categories:', error);
    });
  }
  

  onSubmit(): void {
    if (this.expenseForm.valid) {
      if (this.isEditMode) {
        this.expenseService.updateExpense(this.expenseId!, this.expenseForm.value).subscribe(() => {
          this.router.navigate(['/expenses']);
        });
      } else {
        this.expenseService.addExpense(this.expenseForm.value).subscribe(() => {
          this.router.navigate(['/expenses']);
        });
      }
    }
  }
}
