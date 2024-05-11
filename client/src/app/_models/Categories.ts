export interface Category {
  id: number;
  categoryName: string;
  categoryDescription: string;
  subCategoriesDto: SubCategory[];
}

export interface SubCategory {
  id: number;
  subCategoryName: string;
  image: string;
  subCategoryDescription: string;
  categoryId: number;
  products: string[];
}