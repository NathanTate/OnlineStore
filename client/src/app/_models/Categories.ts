export interface Category {
  id: number;
  categoryName: string;
  categoryDescription: string;
}

export interface CategoryWithSubGroups extends Omit<Category, 'subCategoriesDto'> {
  subcategoryGroups: SubCategoryGroups[];
}

export interface SubCategory {
  id: number;
  subCategoryName: string;
  subCategoryDescription: string;
  categoryId: number;
  groupName: string;
  products: string[];
}

export interface SubCategoryGroups {
  groupName: string;
  subcategories: SubCategory[];
}