export interface Category {
  Id: number;
  categoryName: string;
  categoryDescription: string;
  subCategoriesDto: SubCategory[];
}

export interface SubCategory {
  Id: number;
  subCategoryName: string;
  image: string;
  subCategoryDescription: string;
  categoryId: number;
  products: string[];
}