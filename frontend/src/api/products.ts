import type { Product, ProductListResponse } from "../types/product";
const API_URL = import.meta.env.VITE_API_URL;

export interface ProductFilters {
  search: string;
  category: string;
  minPrice: string;
  maxPrice: string;
  page: number;
}

export async function getProducts(filters: ProductFilters): Promise<ProductListResponse> {
  const params = new URLSearchParams();

  params.set("page", String(filters.page));
  params.set("pageSize", "12");

  if (filters.search !== "") {
    params.set("search", filters.search);
  }
  if (filters.category !== "") {
    params.set("category", filters.category);
  }
  if (filters.minPrice !== "") {
    params.set("minPrice", filters.minPrice);
  }
  if (filters.maxPrice !== "") {
    params.set("maxPrice", filters.maxPrice);
  }

  const response = await fetch(API_URL + "/products?" + params.toString());

  if (!response.ok) {
    throw new Error("Greška pri dohvatu proizvoda");
  }

  const data: ProductListResponse = await response.json();
  return data;
}

export async function getCategories(): Promise<string[]> {
  const response = await fetch(API_URL + "/products/categories");

  if (!response.ok) {
    throw new Error("Greška pri dohvatu kategorija");
  }

  const data: string[] = await response.json();
  return data;
}
export async function getProductById(id: string): Promise<Product> {
  const response = await fetch(API_URL + "/products/" + id);

  if (!response.ok) {
    throw new Error("Proizvod nije pronađen");
  }

  const data: Product = await response.json();
  return data;
}