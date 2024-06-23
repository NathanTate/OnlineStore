export interface Category {
  id: number;
  categoryName: string;
  categoryDescription: string;
  subCategoriesDto: SubCategory[];
}

export interface CategoryWithSubGroups extends Omit<Category, 'subCategoriesDto'> {
  subcategoryGroups: SubCategoryGroups[];
}

export interface SubCategory {
  id: number;
  subCategoryName: string;
  image: string;
  subCategoryDescription: string;
  categoryId: number;
  products: string[];
}

export interface SubCategoryGroups {
  groupName: string;
  subcategories: SubCategory[];
}