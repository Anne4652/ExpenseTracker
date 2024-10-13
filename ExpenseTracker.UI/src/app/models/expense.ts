export interface Expense {
    id?: number;
    amount: number;
    description: string;
    date: string;
    categoryId: number;
    categoryName?: string;
  }
  