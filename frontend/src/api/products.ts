import type { Product, ProductListResponse } from "../types/product";
const API_URL = import.meta.env.VITE_API_URL;

export async function getProducts(page: number): Promise<ProductListResponse> {
  const response = await fetch(API_URL + "/products?page=" + page + "&pageSize=12");

  if (!response.ok) {
    throw new Error("Greška pri dohvatu proizvoda");
  }

  const data: ProductListResponse = await response.json();
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